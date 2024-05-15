namespace Data
{
    public class Passenger
    {
        public enum SexValue
        {
            Male = 0,
            Female = 1
        }

        public bool Survived { get; set; }
        public int PClass { get; set; }
        public string Name { get; set; }
        public SexValue Sex { get; set; }
        public decimal Age { get; set; }
        public int SiblingsOrSpose    { get; set; }
        public int ParentOrChildren { get; set; }
        public decimal Fare { get;set; }

        public static List<Passenger> LoadFromFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var passengers = new List<Passenger>();
            var count = 0;
            foreach (var line in lines)
            {
                if (count == 0) { count++; continue; }
                var values = line.Split(',');
                var passenger = new Passenger
                {
                    Survived = values[0] == "1",
                    PClass = int.Parse(values[1]),
                    Name = values[2],
                    Sex = values[3] == "male" ? SexValue.Male : SexValue.Female,
                    Age = decimal.Parse(values[4]),
                    SiblingsOrSpose = int.Parse(values[5]),
                    ParentOrChildren = int.Parse(values[6]),
                    Fare = decimal.Parse(values[7])
                };

                passengers.Add(passenger);
            }

            return passengers;
        }

    }
}