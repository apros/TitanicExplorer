namespace Data.Test
{
    public class DataTests
    {
        public DataTests() 
        {
            this.SampleDataPath = Path.GetTempFileName();
            //File.WriteAllText(this.SampleDataPath, Resource.passengers);
        }

        public string SampleDataPath { get; set; }

        [Fact]
        public void LoadData()
        {
            var passengers = Passenger.LoadFromFile("titanic.csv");

            Assert.Equal(887, passengers.Count());

            Assert.Equal("Mr. Owen Harris Braund", passengers.First().Name);
        }
    }
}
