using System;

public class SuburbProfile
{
    public string Suburb { get; set; }
    
    //Price Data
    public int HouseMedianPrice { get; set; }
    public int HouseMedianRent { get; set; }
    public float HouseMedianYield { get; set; }
    
    public int UnitMedianPrice { get; set; }
    public int UnitMedianRent { get; set; }
    public float UnitMedianYield { get; set; }

    public int UnitMedianPrice1Br { get; set; }
    public int UnitMedianRent1Br { get; set; }
    public float UnitMedianYield1Br { get; set; }

    public int HouseMedianPrice2Br { get; set; }
    public int HouseMedianRent2Br { get; set; }
    public float HouseMedianYield2Br { get; set; }

    public int UnitMedianPrice2Br { get; set; }
    public int UnitMedianRent2Br { get; set; }
    public float UnitMedianYield2Br { get; set; }
    
    public int HouseMedianPrice3Br { get; set; }
    public int HouseMedianRent3Br { get; set; }
    public float HouseMedianYield3Br { get; set; }

    public int UnitMedianPrice3Br { get; set; }
    public int UnitMedianRent3Br { get; set; }
    public float UnitMedianYield3Br { get; set; }

    public int HouseMedianPrice4Br { get; set; }
    public int HouseMedianRent4Br { get; set; }
    public float HouseMedianYield4Br { get; set; }
    
    //Demand
    public int VisitsPerProperty { get; set; }
    public int StateVisitsPerProperty { get; set; }

    //Sold this year
    public int House2BrSoldThisYear { get; set; }
    public int House3BrSoldThisYear { get; set; }
    public int House4BrSoldThisYear { get; set; }
    public int Unit1BrSoldThisYear { get; set; }
    public int Unit2BrSoldThisYear { get; set; }
    public int Unit3BrSoldThisYear { get; set; }

    //Days on market
    public int House2BrDaysOnMarket { get; set; }
    public int House3BrDaysOnMarket { get; set; }
    public int House4BrDaysOnMarket { get; set; }
    public int Unit1BrDaysOnMarket { get; set; }
    public int Unit2BrDaysOnMarket { get; set; }
    public int Unit3BrDaysOnMarket { get; set; }

    //Auction Clearance Rate
    public int House2BrClearanceRate { get; set; }
    public int House3BrClearanceRate { get; set; }
    public int House4BrClearanceRate { get; set; }
    public int Unit1BrClearanceRate { get; set; }
    public int Unit2BrClearanceRate { get; set; }
    public int Unit3BrClearanceRate { get; set; }

    public DateTime PriceDataUpdateDate { get; set; }
    public DateTime RentDataUpdateDate { get; set; }
}