using CommonFilter;
using Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;
using static Data.Passenger;

namespace TitanicFilter.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            var simpleDataPath = "titanic.csv"; 
            this.Passengers = Passenger.LoadFromFile(simpleDataPath);
        }

        public IEnumerable<Passenger> Passengers { get; set; }

        public void OnGet()
        {

        }

        public void OnPost() {
            var survived = Request.Form["survived"] != "" ? ParseSurvived(Request.Form["survived"]) : null;
            var pClass = ParseNullInt(Request.Form["pClass"]);
            var sex = Request.Form["sex"] != "" ? ParseSex(Request.Form["sex"]) : null;
            var age = ParseNullDecimal(Request.Form["age"]);
            var minimumFare = ParseNullDecimal(Request.Form["minimumFare"]);
            this.Passengers = FilterPassengers(survived, pClass, sex, age, minimumFare);
        }

        private IEnumerable<Passenger> FilterPassengers(bool? survived, int? pClass, SexValue? sex, decimal? age, decimal? minimumFare)
        {
            Expression? currentExpression = null;
            var passengerParameter = Expression.Parameter(typeof(Passenger));
            if (survived != null)
            {
                currentExpression = CommonFilterLib.CreateExpression(survived.Value, null, "Survived", passengerParameter);
            }
            if (pClass != null)
            {
                currentExpression = CommonFilterLib.CreateExpression(pClass.Value, currentExpression, "PClass", passengerParameter);
            }
            if (sex != null)
            {
                currentExpression = CommonFilterLib.CreateExpression(sex.Value, currentExpression, "Sex", passengerParameter);
            }
            if (age != null)
            {
                currentExpression = CommonFilterLib.CreateExpression(age.Value, currentExpression, "Age", passengerParameter);
            }
            if (minimumFare != null)
            {
                currentExpression = CommonFilterLib.CreateExpression(minimumFare.Value, currentExpression, "Fare", passengerParameter, ">");
            }

            if (currentExpression != null)
            {
                var expr = Expression.Lambda<Func<Passenger, bool>>(currentExpression, false, new List<ParameterExpression> { passengerParameter});
                var func = expr.Compile();
                this.Passengers = this.Passengers.Where(func);
            }

            return this.Passengers;
        }

        private bool? ParseSurvived(string value) { return (value == "Survived") ? true : value == "Perished" ? false : null; }
        private SexValue? ParseSex(string value) { return (value == "male") ? SexValue.Male : value == "female" ? SexValue.Female : null; }
        private int? ParseNullInt(string value) {
            if (int.TryParse(value, out var intValue)) { return intValue; } else { return null; }
        }
        private decimal? ParseNullDecimal(string value) { 
            if(decimal.TryParse(value, out var decimalValue)) {  return decimalValue; } else { return null; }
        }
    }
}