using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TNS.Importer.Tests.PlayTest
{
    [Trait("Data aggregation test", "tests against specific data sets")]
    public class StoreAnalyticsTests
    {
        public class StoreData 
        {
            public int RespId { get; set; }
            public int Q1 { get; set; }
            public string StoreId { get; set; }
            public string AreaId { get; set; }
        }

        List<StoreData> _testData;
        
        public StoreAnalyticsTests()
        {
           _testData = new List<StoreData>{
                new StoreData { RespId = 1, Q1 = 3, AreaId = "London", StoreId = "L012" },
                new StoreData { RespId = 2, Q1 = 3, AreaId = "London", StoreId = "L012" },
                new StoreData { RespId = 3, Q1 = 3, AreaId = "London", StoreId = "L012" },
                new StoreData { RespId = 4, Q1 = 3, AreaId = "London", StoreId = "L012" },
                new StoreData { RespId = 5, Q1 = 3, AreaId = "London", StoreId = "L02" },
                new StoreData { RespId = 6, Q1 = 3, AreaId = "London", StoreId = "L02" },
                new StoreData { RespId = 7, Q1 = 3, AreaId = "London", StoreId = "L02" },
                new StoreData { RespId = 8, Q1 = 3, AreaId = "London", StoreId = "L02" },
                new StoreData { RespId = 9, Q1 = 1, AreaId = "London", StoreId = "L02" },
                
                new StoreData { RespId = 10, Q1 = 1, AreaId = "Cumbria", StoreId = "C015" },
                new StoreData { RespId = 11, Q1 = 4, AreaId = "Cumbria", StoreId = "C015" },
                new StoreData { RespId = 12, Q1 = 4, AreaId = "Cumbria", StoreId = "C015" },
                new StoreData { RespId = 13, Q1 = 4, AreaId = "Cumbria", StoreId = "C015" },
                new StoreData { RespId = 14, Q1 = 5, AreaId = "Cumbria", StoreId = "C015" }
            };
        }

        [Fact(DisplayName = "Can obtain average score for a given store")]
        public void CanObtainAverageStoreForAGivenStore()
        {
            string storeId = "L012";
            var dataForStore = _testData
                                        .Where(e => e.StoreId.Equals(storeId, StringComparison.InvariantCultureIgnoreCase))
                                        .ToList();
            double storeCount = dataForStore.Count();
            double totalScoreForStores = dataForStore.Sum(d => d.Q1);
            double average = totalScoreForStores/storeCount;
            double storeAv = StoreAverage(storeId);
            Assert.True(storeAv == average);
        }


        [Fact(DisplayName = "Can obtain averages for all stores")]
        public void GetAveragesForAllStores()
        {
            _testData.GroupBy(d => d.StoreId)
                .Select(e => new
                {
                    store = e.Key,
                    scoreAvg = e.Average(g => g.Q1)
                });
                
        }

        public double NationalAverage()
        { 
            return _testData
                        .Average(d=>d.Q1);
        }

        public double StoreAverage(string storeId)
        {
            return _testData
                        .Where(d => d.StoreId.Equals(storeId, StringComparison.InvariantCultureIgnoreCase))
                        .Average(d => d.Q1);
                        


            //return _testData
            //  .Where(d => d.StoreId.Equals(storeId, StringComparison.InvariantCultureIgnoreCase))
            //  .GroupBy(d => d.StoreId)
            //  .Select(g => new
            //  {
            //      store = g.Key,
            //      avg = g.Average(s => s.Q1),
            //      count = g.Count(),
            //      sum = g.Sum(s => s.RespId),
            //  });
             
        }

        public double StoreAverageInArea(string areaId)
        {
            // average score in an area
            return _testData
                        .Where(stores => stores.AreaId==areaId)
                        .Average(d => d.Q1);


            var highestStoreAverage = _testData
                        .GroupBy(s => s.StoreId)
                        .Select(s => new
                        {
                            store = s.Key,
                            average = s.Average(sc => sc.Q1)
                        }).OrderByDescending(st => st.average).First().average;
            


            //or

            // average score FOR A STORE in an area - i.e. the average of the average !
            return _testData
                .Where(stores => stores.AreaId == areaId)
                .GroupBy(area => area.StoreId)
                .Select(group => new
                {
                    store = group.Key,
                    storeAverage = group.Average(s => s.Q1)
                }).Average(ar=>ar.storeAverage);
        }




    }
}
