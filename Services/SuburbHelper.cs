using System.IO;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
public class SuburbHelper
{    
    public List<string> GetSuburbList(string filePath)
    {
        var suburbList = new List<string>();
        var suburbLines = File.ReadAllLines(filePath);

        foreach(var line in suburbLines)
        {
            var suburbs = line.Split(",");
            suburbList.AddRange(suburbs);
        }
        
        return suburbList;
    }

    public SuburbProfile GetSuburbProfile(string suburbName)
    {
        var web = new HtmlWeb();
        var htmlDocument = web.Load($"https://www.realestate.com.au/neighbourhoods/{suburbName}-nsw");
        if (htmlDocument.DocumentNode.SelectSingleNode("//title").InnerText == "404")
        { 
            return null;
        };

        var suburbProfile = new SuburbProfile();
        suburbProfile.Suburb = suburbName;
        
        //var medianPriceNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='median-price']");
        
        //units
        var unitDataNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='slide-section median-price-subsections units']");
        var unitPriceNodes = unitDataNode.SelectNodes(".//div[@class='price strong']").ToArray();

        var cleansedUnitPrices = unitPriceNodes.Select(x => x.InnerText.Remove(0,1).Replace(",","").Replace("PW","").Replace(" ","")).ToArray();
        if (cleansedUnitPrices.Count() == 6)
        {
            int b1,b2,b3 = 0;
            int r1,r2,r3 = 0;
            int.TryParse(cleansedUnitPrices[0], out b1);
            int.TryParse(cleansedUnitPrices[1], out b2);
            int.TryParse(cleansedUnitPrices[2], out b3);
            int.TryParse(cleansedUnitPrices[3], out r1);
            int.TryParse(cleansedUnitPrices[4], out r2);
            int.TryParse(cleansedUnitPrices[5], out r3);
            suburbProfile.UnitMedianPrice1Br = b1;
            suburbProfile.UnitMedianPrice2Br = b2;
            suburbProfile.UnitMedianPrice3Br = b3;
            suburbProfile.UnitMedianRent1Br = r1;
            suburbProfile.UnitMedianRent2Br = r2;
            suburbProfile.UnitMedianRent3Br = r3;

            if (r1 != 0 && b1 != 0){suburbProfile.UnitMedianYield1Br = (float)r1*52/b1;}
            if (r2 != 0 && b2 != 0){suburbProfile.UnitMedianYield2Br = (float)r2*52/b2;}
            if (r3 != 0 && b3 != 0){suburbProfile.UnitMedianYield3Br = (float)r3*52/b3;}
        }

        //houses
        var houseDataNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='slide-section default-slide-section median-price-subsections houses']");
        var housePriceNodes = houseDataNode.SelectNodes(".//div[@class='price strong']").ToArray();

        var cleansedHousePrices = housePriceNodes.Select(x => x.InnerText.Remove(0,1).Replace(",","").Replace("PW","").Replace(" ","")).ToArray();
        if (cleansedHousePrices.Count() == 6)
        {
            int b1,b2,b3 = 0;
            int r1,r2,r3 = 0;
            int.TryParse(cleansedHousePrices[0], out b1);
            int.TryParse(cleansedHousePrices[1], out b2);
            int.TryParse(cleansedHousePrices[2], out b3);
            int.TryParse(cleansedHousePrices[3], out r1);
            int.TryParse(cleansedHousePrices[5], out r3);
            int.TryParse(cleansedHousePrices[4], out r2);
            suburbProfile.HouseMedianPrice2Br = b1;
            suburbProfile.HouseMedianPrice3Br = b2;
            suburbProfile.HouseMedianPrice4Br = b3;
            suburbProfile.HouseMedianRent2Br = r1;
            suburbProfile.HouseMedianRent3Br = r2;
            suburbProfile.HouseMedianRent4Br = r3;

            if (r1 != 0 && b1 != 0){suburbProfile.HouseMedianYield2Br = (float)r1*52/b1;}
            if (r2 != 0 && b2 != 0){suburbProfile.HouseMedianYield3Br = (float)r2*52/b2;}
            if (r3 != 0 && b3 != 0){suburbProfile.HouseMedianYield4Br = (float)r3*52/b3;}
        }

        return suburbProfile;
    }
}