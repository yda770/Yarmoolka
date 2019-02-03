
using System.Collections.Generic;
using System.Linq;

using System.IO;


using NReco.CF.Taste.Model;
using NReco.CF.Taste.Impl.Model.File;
using NReco.CF.Taste.Impl.Eval;
using NReco.CF.Taste.Eval;
using NReco.CF.Taste.Impl.Similarity;
using NReco.CF.Taste.Impl.Neighborhood;
using NReco.CF.Taste.Impl.Recommender;
using NReco.CF.Taste.Impl.Recommender.SVD;
using NReco.CF.Taste.Impl.Model;
using NReco.CF.Taste.Recommender;

using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;

namespace Controllers
{

    public class RecommenderController : Controller
    {

        private IHostingEnvironment _env;
        private IMemoryCache _cache;

        public RecommenderController(IHostingEnvironment env, IMemoryCache cache)
        {
            _env = env;
            _cache = cache;
        }

        public ActionResult Index()
        {
            return View();
        }

        static IDataModel dataModel;

        public ActionResult GetRecommendedFilms(string filmIdsJson)
        {
            var filmIds = JsonConvert.DeserializeObject<long[]>(filmIdsJson);

            var dataModel = GetDataModel();

            // recommendation is performed for the user that is missed in the preferences data
            var plusAnonymModel = new PlusAnonymousUserDataModel(dataModel);
            var prefArr = new GenericUserPreferenceArray(filmIds.Length);
            prefArr.SetUserID(0, PlusAnonymousUserDataModel.TEMP_USER_ID);
            for (int i = 0; i < filmIds.Length; i++)
            {
                prefArr.SetItemID(i, filmIds[i]);

                // in this example we have no ratings of Yarmoolkas preferred by the user
                prefArr.SetValue(i, 5); // lets assume max rating
            }
            plusAnonymModel.SetTempPrefs(prefArr);

            var similarity = new LogLikelihoodSimilarity(plusAnonymModel);
            var neighborhood = new NearestNUserNeighborhood(15, similarity, plusAnonymModel);
            var recommender = new GenericUserBasedRecommender(plusAnonymModel, neighborhood, similarity);
            var recommendedItems = recommender.Recommend(PlusAnonymousUserDataModel.TEMP_USER_ID, 5, null);

            return Json(recommendedItems.Select(ri => new Dictionary<string, object>() {
                {"film_id", ri.GetItemID() },
                {"rating", ri.GetValue() },
            }).ToArray());
        }

        /// <summary>
        /// Loads data model (preferences data) from the file. In the same manner data can be loaded from SQL database (or MongoDb).
        /// </summary>
        /// <remarks>
        /// Data model is cached to avoid CSV parsing on each request.
        /// </remarks>
        IDataModel GetDataModel()
        {

            var webRoot = _env.WebRootPath;
            var file = System.IO.Path.Combine(webRoot, "ratings.csv");

            var cacheKey = "RecommenderDataModel:" + file;

            var dataModel = _cache.Get(cacheKey) as IDataModel;
            if (dataModel == null)
            {
                dataModel = new FileDataModel(file, false, FileDataModel.DEFAULT_MIN_RELOAD_INTERVAL_MS, false);
                _cache.Set(cacheKey, dataModel);
            }
            return dataModel;
        }

        public ActionResult GetYarmoolkas()
        {

            var webRoot = _env.WebRootPath;
            var file = System.IO.Path.Combine(webRoot, "movies.csv");

            var csv = new CsvReader(new StreamReader(file));
            var records = csv.GetRecords<YarmoolkaRecord>();
            return Json(records);
        }

        public class YarmoolkaRecord
        {
            public int YarmoolkaId { get; set; }
            public string title { get; set; }
        }

    }
}
