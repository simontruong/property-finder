using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using System.Globalization;
using ServiceStack.Text;
using ServiceStack;

namespace PropertyFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuburbsController : ControllerBase
    {
        private readonly SuburbHelper _suburbHelper;

        public SuburbsController()
        {
            _suburbHelper = new SuburbHelper();
        }

        // GET api/values
        [HttpGet]
        public ActionResult<List<SuburbProfile>> GetAll()
        {
            var web = new HtmlWeb();

            var suburbs = _suburbHelper.GetSuburbList("SuburbList.txt");
            var suburbProfiles = new List<SuburbProfile>();
            foreach(var suburbName in suburbs)
            {
                Console.WriteLine(suburbName);

                var suburbProfile = _suburbHelper.GetSuburbProfile(suburbName);
                if (suburbProfile != null)
                {
                    suburbProfiles.Add(suburbProfile);
                }
            }

            var csvHelper = new CsvHelper();
            csvHelper.WriteCsv(suburbProfiles, "suburb-data.csv");
            
            return suburbProfiles;
        }

        [HttpGet("favourites")]
        public ActionResult<List<SuburbProfile>> GetFavourites()
        {
            var web = new HtmlWeb();

            var suburbs = _suburbHelper.GetSuburbList("FavouriteList.txt");
            var suburbProfiles = new List<SuburbProfile>();
            foreach(var suburbName in suburbs)
            {
                Console.WriteLine(suburbName);
                                
                var suburbProfile = _suburbHelper.GetSuburbProfile(suburbName);
                if (suburbProfile != null)
                {
                    suburbProfiles.Add(suburbProfile);
                }
            }

            var csvHelper = new CsvHelper();
            csvHelper.WriteCsv(suburbProfiles, "favourite-suburb-data.csv");
            
            return suburbProfiles;
        }

        [HttpGet("{suburbName}")]
        public ActionResult<SuburbProfile> Get(string suburbName)
        {
            var suburbProfile = _suburbHelper.GetSuburbProfile(suburbName);
            return suburbProfile;
        }
    }
}
