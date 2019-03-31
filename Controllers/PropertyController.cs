using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using System.Globalization;

namespace PropertyFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Property>> Get()
        {
            var web = new HtmlWeb();
            var htmlDocument = web.Load("https://www.domain.com.au/auction-results/sydney/");

            //Sold Date
            var soldDateData = htmlDocument.DocumentNode.SelectSingleNode("//h2[@class='sales-results-hero-content__heading']").InnerText;
            var splitDateData = soldDateData.Split(" ");
            var cleanDay = splitDateData[1].Replace("th","").Replace("nd","").Replace("rd","").Replace("st","");
            soldDateData = $"{cleanDay} {splitDateData[2]} {splitDateData[3]}";
            
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime soldDate;
            DateTime.TryParseExact(soldDateData, "d MMMM yyyy", enUS, DateTimeStyles.None, out soldDate);
            

            var properties = new List<Property>();
            var suburbs = htmlDocument.DocumentNode.SelectNodes("//div[@class='suburb-listings']");
            foreach(var suburb in suburbs)
            {
                var suburbName = suburb.SelectSingleNode(".//h6[@class='suburb-listings__heading']").InnerText;
                
                var propertyListings = suburb.SelectNodes(".//a[@class='auction-details']");
                foreach (var propertyListing in propertyListings)
                {
                    
                    
                    var address = propertyListing.SelectSingleNode(".//span[@class='auction-details__address']").InnerText;
                    var priceData = propertyListing.SelectSingleNode(".//span[@class='auction-details__price']").InnerText;
                    bool passedIn = false;
                    bool priceWithheld = false;
                    
                    //price logic
                    decimal price = 0;
                    switch(priceData.ToLower())
                    {
                        case "price withheld":
                            priceWithheld = true;
                            break;
                        case "passed in":
                            passedIn = true;
                            break;
                        default:
                            var numericOnly = priceData.Substring(1, priceData.Length - 2);
                            if (priceData.EndsWith("k"))
                            {
                                decimal shortPrice = 0;
                                decimal.TryParse(numericOnly, out shortPrice);
                                price = shortPrice * 1000;
                            }
                            if (priceData.EndsWith("m"))
                            {
                                decimal shortPrice = 0;
                                decimal.TryParse(numericOnly, out shortPrice);
                                price = shortPrice * 1000000;
                            }
                            break;
                    }

                    //property details
                    var propertyType = string.Empty;
                    var beds = 0;
                    var baths = 0;
                    var parking = 0;
                    var landsize = string.Empty;
                    var url = propertyListing.GetAttributeValue("href", null);
                    var propertyDetailDocument = web.Load(url);
                    var propertyFeaturesNode = propertyDetailDocument.DocumentNode.SelectSingleNode("//div[@class='property-features__default-wrapper']");
                    if (propertyFeaturesNode != null)
                    {
                        var propertyFeatures = propertyFeaturesNode.SelectNodes(".//span[@class='property-feature__feature-text-container']");
                        foreach (var pf in propertyFeatures)
                        {
                            var lowerPropertyFeature = pf.InnerText.ToLower();
                            //Console.WriteLine(pf.InnerText);
                            if (lowerPropertyFeature.EndsWith("beds"))
                            {
                                int.TryParse(lowerPropertyFeature.Split(" ")[0], out beds);
                                
                            }
                            if (lowerPropertyFeature.EndsWith("baths"))
                            {
                                int.TryParse(lowerPropertyFeature.Split(" ")[0], out baths);
                            }
                            if (lowerPropertyFeature.EndsWith("parking"))
                            {
                                int.TryParse(lowerPropertyFeature.Split(" ")[0], out parking);
                            }
                            
                            if(lowerPropertyFeature.Length>=3 && lowerPropertyFeature.Substring(lowerPropertyFeature.Length-3,1) == "m")
                            {
                                landsize = pf.InnerText;
                            }
                        }
                        
                        var keyFeatures = propertyDetailDocument.DocumentNode.SelectNodes("//div[@class='listing-details__key-features--item listing-details__key-features--property-type']");
                        if (keyFeatures != null)
                        {
                            foreach (var kf in keyFeatures)
                            {
                                var lowerKeyFeature = kf.InnerText.ToLower();
                                if (lowerKeyFeature.StartsWith("property type"))
                                {
                                    propertyType = kf.InnerText.Substring(13, lowerKeyFeature.Length-13);
                                }
                            }
                        }
                    }
                    
                    


                    //Console.WriteLine($"{suburbName}:{address}:{priceData}");
                    var property = new Property{
                        Suburb = suburbName,
                        Address = address,
                        Price = price,
                        PassedIn = passedIn,
                        PriceWithheld = priceWithheld,
                        PropertyType = propertyType,
                        Bedrooms = beds,
                        Bathrooms = baths,
                        Parking = parking, 
                        LandSize = landsize,
                        PostUrl = url,
                        SoldDate = soldDate                     
                    };

                    properties.Add(property);
                }
            }
            //var nodes = htmlDocument.DocumentNode.SelectNodes("//h6[@class='suburb-listings__heading']");
            
            //return nodes.Select(x => new Property(){Suburb = x.InnerText}).ToList();
            return properties;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
