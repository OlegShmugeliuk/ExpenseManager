using Microsoft.AspNetCore.Mvc;
using ExpenseManager; // Assuming InfoBody class is in the Models namespace
using System.Collections.Generic;
using Service;
using ParserCurrenty;
using System.Linq;
using Currenty;
using AngleSharp.Browser;
using System.Globalization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Text.RegularExpressions;
using System.Text.Json;
using Microsoft.AspNetCore.Routing;





namespace ExpenseManager.Controllers
{

	[Route("[controller]")]
	public class MainController : Controller
	{
		private static List<InfoBody> _infoBody = new List<InfoBody>()
		{
			//new InfoBody { Body = "Test sosige",ExpenditureType="Housing", CurrentyType = "UAH", Price = 53.45},
			//new InfoBody {  Body = "Test pizza",ExpenditureType="Healthcare" ,CurrentyType = "UAH", Price = 100.0 },
			//new InfoBody {  Body = "Test pizza",ExpenditureType="Healthcare" ,CurrentyType = "UAH", Price = 100.0,postDateTime = new DateTime(2023, 11, 01) },
			//new InfoBody { Body = "Test water" ,ExpenditureType="Utilities",CurrentyType = "UAH", Price = 0.45, postDateTime = new DateTime(2024, 01, 13)},
   //         new InfoBody { Body = "Test water" ,ExpenditureType="Utilities",CurrentyType = "UAH", Price = 0.45, postDateTime = new DateTime(2024, 01, 10)},
            new InfoBody {  Body = "Test potato",ExpenditureType="Housing" ,CurrentyType = "UAH", Price = 190.45}
		};
        
        private static List<IncomePerson> _incom = new List<IncomePerson>()
		{
			//new IncomePerson {Income = 12.54, Currency = "UAH", SourceOfIncome="Job"},
			//new IncomePerson {Income = 122.54, Currency = "UAH", SourceOfIncome="Freelance", IncomeTime = new DateTime(2023, 11, 01) },
   //         new IncomePerson {Income = 122.54, Currency = "UAH", SourceOfIncome="Freelance", IncomeTime = new DateTime(2024, 03, 10) },
   //         new IncomePerson {Income = 999.54, Currency = "UAH", SourceOfIncome="Freelance", IncomeTime = new DateTime(2024, 01, 10) },
   //         new IncomePerson {Income = 120002.54, Currency = "UAH", SourceOfIncome="Job", IncomeTime = new DateTime(2022, 03, 10) },
   //         new IncomePerson {Income = 1.54, Currency = "USD", SourceOfIncome="Freelance"},
			//new IncomePerson {Income = 1200.54, Currency = "PLN", SourceOfIncome="Business"},            
        };

		public static Dictionary<string, double> result = new Dictionary<string, double>();


		public MainController()
		{

		}

		[Route("/index")]
		public IActionResult Index()
		{
			// Assuming you have a method to transform _infoBody to DictionaryForHomePage
			var transformedData = TransformData(_infoBody);            
            return View(transformedData);
		}

		// Add a method to transform InfoBody to DictionaryForHomePage
		private IEnumerable<DictionaryForHomePage> TransformData(List<InfoBody> infoBodyData)
		{
			var transformedData = infoBodyData.Select(item => new DictionaryForHomePage
			{
				ExpenditureType = item.ExpenditureType,
				TotalPrice = item.Price
			});

			return transformedData;
		}


		private IEnumerable<DictionaryForHomePage> GenerateExpenditureInfos(IEnumerable<InfoBody> infoBodies, Func<InfoBody, bool> predicate)
		{
			return infoBodies
				.Where(predicate)
				.GroupBy(item => item.ExpenditureType)
				.Select(group => new DictionaryForHomePage
				{
					ExpenditureType = group.Key,
					TotalPrice = group.Sum(grpItem => grpItem.Price),
					CurrencyType = group.First().CurrentyType,
					PostDate = group.First().postDateTime
				})
				.Distinct();
		}

		[HttpGet]
		
		[Route("[action]")]
		public async Task<IActionResult> Start()
		{
			ViewBag.DateInfo = "";
			CultureInfo culture = new CultureInfo("en-US");

			Dictionary<string, string> month = new Dictionary<string, string>();
			List<string> Year = new List<string>();
			int i = 0;
			if (_infoBody != null)
			{
				foreach (var item in _infoBody)
				{
					if (item.postDateTime != null && !month.ContainsKey(item.postDateTime.Value.ToString("yyyy.MM.dd")))
					{
						month[item.postDateTime.Value.ToString("MMMM yyyy", culture)] = item.postDateTime.Value.ToString("yyyy.MM.dd");

					}
					string year = item.postDateTime.Value.Year.ToString();
					if (!Year.Contains(year))
					{
						Year.Add(year);
					}
				}
			}
			ViewBag.Month = month;
			ViewBag.Year = Year;

			

			DateTime PostDateTime = DateTime.Now.Date;

			IEnumerable<DictionaryForHomePage> expenditureInfos = GenerateExpenditureInfos(
			_infoBody, item => item.postDateTime?.Date == PostDateTime.Date);
			ViewBag.DateInfo = PostDateTime.Date.ToString("dd-MM-yyyy");



			

            return View(expenditureInfos);
		}


		[HttpPost]
		[Route("[action]")]
		public IActionResult Start(DateTime? PostDateTime = null, DateTime? end = null, DateTime? DateOfMonth = null, string? DateOfYear = null)
		{
			CultureInfo culture = new CultureInfo("en-US");
			ViewBag.DateInfo = "";
			

			Dictionary<string, string> month = new Dictionary<string, string>();
			List<string> Year = new List<string>();
			int i = 0;
			if (_infoBody != null)
			{
				foreach (var item in _infoBody)
				{
					if (item.postDateTime != null && !month.ContainsKey(item.postDateTime.Value.ToString("yyyy.MM.dd")))
					{
						month[item.postDateTime.Value.ToString("MMMM yyyy", culture)] = item.postDateTime.Value.ToString("yyyy.MM.dd");

					}
					string year = item.postDateTime.Value.Year.ToString();
					if (!Year.Contains(year))
					{
						Year.Add(year);
					}
				}
			}
			ViewBag.Month = month;
			ViewBag.Year = Year;


			if (DateOfMonth.HasValue)
			{

				IEnumerable<DictionaryForHomePage> expenditureInfosMonth = GenerateExpenditureInfos(
			_infoBody, item => item.postDateTime?.Date.Month == DateOfMonth.Value.Month);

                ViewBag.DateInfo = DateOfMonth.Value.ToString("MMMM yyyy", culture);
				ViewBag.TypePeriod = "month";
                return View(expenditureInfosMonth);
			}


			if (DateOfYear != null)
			{
				IEnumerable<DictionaryForHomePage> expenditureInfosYear = GenerateExpenditureInfos(_infoBody, item => item.postDateTime?.Year.ToString() == DateOfYear);
				ViewBag.DateInfo = DateOfYear + " year";
                ViewBag.TypePeriod = "year";
                return View(expenditureInfosYear);
			}



			
			if (PostDateTime.HasValue && end.HasValue)
			{
				IEnumerable<DictionaryForHomePage> expenditure = GenerateExpenditureInfos(_infoBody, item => item.postDateTime?.Date >= PostDateTime.Value.Date && item.postDateTime?.Date <= end.Value.Date);
                ViewBag.DateInfo = PostDateTime.Value.ToString("dd-MM-yyyy");

                return View(expenditure);
			}
            

            if (PostDateTime.HasValue && PostDateTime.Value.ToString("dd.MM.yyyy") == "01.01.0001")
			{
				ViewBag.DateInfo = "all time";

                IEnumerable<DictionaryForHomePage> Infos = _infoBody
			 .GroupBy(item => item.ExpenditureType)
			 .Select(group => new DictionaryForHomePage
			 {
				 ExpenditureType = group.Key,
				 TotalPrice = group.Sum(grpItem => grpItem.Price),
				 CurrencyType = group.First().CurrentyType,
				 PostDate = group.First().postDateTime
			 })
			 .Distinct();
                ViewBag.TypePeriod = "all time";

                return View(Infos);
			}


			if (!PostDateTime.HasValue)
			{
				PostDateTime = DateTime.Now.Date;
			}


			

			IEnumerable<DictionaryForHomePage> expenditureInfos = GenerateExpenditureInfos(_infoBody, item => item.postDateTime?.Date == PostDateTime.Value.Date);
            ViewBag.DateInfo = PostDateTime.Value.ToString("dd-MM-yyyy");

           

			return View(expenditureInfos);

		}


        [Route("[action]/{type}/{postDate}")]

        public IActionResult InformationAbouType(string typePeriod, string type, DateTime postDate, string DateOnfo)
        {
            CultureInfo culture = new CultureInfo("en-US");
            List<InfoBody> Info = _infoBody.Where(item => item.postDateTime.Value.Date == postDate.Date && item.ExpenditureType == type).ToList();
            ViewBag.InfoType = type;

            if (typePeriod == "month")
			{
                Info = _infoBody.Where(item => item.postDateTime.Value.Date.ToString("MMMM yyyy", culture) == DateOnfo && item.ExpenditureType == type).ToList();
                ViewBag.DateInfo = DateOnfo;
                return View(Info);
            }
            if (typePeriod == "year")
            {
                Info = _infoBody.Where(item => item.postDateTime.Value.Date.ToString("yyyy")+" year" == DateOnfo && item.ExpenditureType == type).ToList();
                ViewBag.DateInfo = DateOnfo;
                return View(Info);
            }
            if (typePeriod == "all time")
            {
                Info = _infoBody.Where(item=>item.ExpenditureType == type).ToList();
                ViewBag.DateInfo = DateOnfo;
                return View(Info);
            }

            if (DateOnfo == "")
            {
                ViewBag.DateInfo = postDate.Date.ToString("dd-MM-yyyy");
            }
            ViewBag.DateInfo = DateOnfo;
            return View(Info);
        }


        [HttpGet]
		[Route("[action]")]
		public IActionResult Information(DateTime? today = null)
		{
			if (!today.HasValue)
			{
				today = DateTime.Now;
			}

			List<InfoBody> infoBodies = _infoBody.Where(item => item.postDateTime?.Date.ToString("dd mm yyyy") == today.Value.Date.ToString("dd mm yyyy")).ToList();

			return View(infoBodies);
		}


		[HttpGet]
		[Route("[action]")]
		public IActionResult Add()
		{
			ViewBag.CurrentyType = new List<string> { "UAH", "USD", "EUR", "PLN" };
			ViewBag.ExpenditureType = new List<string>
			{
				"Housing", "Food", "Transportation", "Healthcare", "Education",
				"Entertainment", "Clothing", "Products",
                "Utilities"
			};

			ViewBag.SourceOfIncome = new List<string>()
			{
				"Job", "Business", "Freelance"
			};


			return View();
		}

		[HttpPost]
		[Route("[action]")]
		public IActionResult AddIncomeInformation(double income, string currency, string sourceOfIncome)
		{
			_incom.Add(new IncomePerson { Income = income, Currency = currency, SourceOfIncome = sourceOfIncome });
			
			return RedirectToAction("InfoIncome");
		}

		[HttpPost]
		[Route("[action]")]
		public IActionResult Add(string body, string expenditureType, double price, string currentyType)
		{
			_infoBody.Add(new InfoBody { Body = body, ExpenditureType = expenditureType, CurrentyType = currentyType, Price = price });
			return RedirectToAction("Start");
		}

		[HttpGet]
		[Route("[action]/{IdBody}")]
		public IActionResult Edit(int IdBody)
		{
			var infoBody = _infoBody.Find(body => body.Id == IdBody);

			if (infoBody != null)
			{
				return View(infoBody);
			}

			return View("/Error");
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<IActionResult> Edit(int itemId, string currentyType, string Type)
		{
			await Console.Out.WriteLineAsync(currentyType + " " + itemId);
			
            double ConvertPrice = 0.0;
            ConvertValue convertValue = new ConvertValue();
            Parser parser = new Parser();

            IncomePerson incomePerson = _incom.FirstOrDefault(item => item.Id == itemId);
			
			
				
            if (incomePerson != null)
			{				
                if (incomePerson.Currency == currentyType)
                {
                    ConvertPrice = incomePerson.Income.Value;

                }
                else
                {
                    if (incomePerson.Currency == "UAH")
                    {
                        result = await parser.ParseForUAH();
                        ConvertPrice = convertValue.ConvertFrom(currentyType, result, incomePerson.Income.Value);

                    }
                    if (incomePerson.Currency == "EUR")
                    {
                        result = await parser.ParseForEUR();
                        result.Add("UAH", 41);
                        ConvertPrice = convertValue.ConvertFrom(currentyType, result, incomePerson.Income.Value);

                    }
                    if (incomePerson.Currency == "USD")
                    {
                        result = await parser.ParseForUSD();
                        ConvertPrice = convertValue.ConvertFrom(currentyType, result, incomePerson.Income.Value);

                    }
                    if (incomePerson.Currency == "PLN")
                    {
                        result = await parser.ParseForPLN();
                        ConvertPrice = convertValue.ConvertFrom(currentyType, result, incomePerson.Income.Value);
                    }

                }
                //var infoBody = _infoBody.Find(body => body.Id == IdBody);
                if (incomePerson != null)
                {

                    incomePerson.Income = ConvertPrice;
                    incomePerson.Currency = currentyType;

                }
                return RedirectToAction("History");
            }
			else
			{

				InfoBody infoBody = _infoBody.FirstOrDefault(item => item.Id == itemId);

				

				if (infoBody.CurrentyType == currentyType)
				{
					ConvertPrice = infoBody.Price;

				}
				else
				{
					if (infoBody.CurrentyType == "UAH")
					{
						result = await parser.ParseForUAH();
						ConvertPrice = convertValue.ConvertFrom(currentyType, result, infoBody.Price);

					}
					if (infoBody.CurrentyType == "EUR")
					{
						result = await parser.ParseForEUR();
						result.Add("UAH", 41);
						ConvertPrice = convertValue.ConvertFrom(currentyType, result, infoBody.Price);

					}
					if (infoBody.CurrentyType == "USD")
					{
						result = await parser.ParseForUSD();
						ConvertPrice = convertValue.ConvertFrom(currentyType, result, infoBody.Price);

					}
					if (infoBody.CurrentyType == "PLN")
					{
						result = await parser.ParseForPLN();
						ConvertPrice = convertValue.ConvertFrom(currentyType, result, infoBody.Price);
					}

				}
				//var infoBody = _infoBody.Find(body => body.Id == IdBody);
				if (infoBody != null)
				{

					infoBody.Price = ConvertPrice;
					infoBody.CurrentyType = currentyType;

				}
                return RedirectToAction("History");
            }
            return RedirectToAction("History");
        }

		

        [HttpPost]
        [Route("[action]")]
        public IActionResult Delete(int IdBody)
        {
            var infoBody = _infoBody.Find(body => body.Id == IdBody);

            if (infoBody != null)
            {
                _infoBody.Remove(infoBody);
            }
			else
			{
				var income = _incom.Find(item => item.Id == IdBody);
				if(income != null)
				{
					_incom.Remove(income);
				}
			}
            
            return RedirectToAction("History");
        }



        

		[Route("[action]")]
		public IActionResult EmptyPage()
		{
			return View();
		}

		[Route("[action]")]
		public IActionResult History()
		{
			CultureInfo culture = new CultureInfo("en-US");

			List<string> DatePost = new List<string>();

			_infoBody = _infoBody.OrderByDescending(item => item.postDateTime).ToList();

			foreach (var item in _infoBody)
			{
				string date = item.postDateTime.Value.ToString("dd MMMM yyyy", culture);
				if (!DatePost.Contains(date))
				{
					DatePost.Add(item.postDateTime.Value.ToString("dd MMMM yyyy", culture));
				}
			}
			foreach (var item in _incom)
			{
				string date = item.IncomeTime.Value.ToString("dd MMMM yyyy", culture);
				if (!DatePost.Contains(date))
				{
					DatePost.Add(item.IncomeTime.Value.ToString("dd MMMM yyyy", culture));
				}
			}

			HistoryViewModel historyViewModel = new HistoryViewModel()
			{
				infoBodies = _infoBody,
				incomePersons = _incom
			};
			


            if (historyViewModel.incomePersons.Count == 0 && historyViewModel.infoBodies.Count == 0)
			{
				return RedirectToAction("EmptyPage");
			}

			ViewBag.DatePost = DatePost;
			return View(historyViewModel);
		}

        [HttpGet]
        [Route("[action]")]
        public IActionResult InfoIncome()
        {
            ViewBag.DateInfo = "";
            CultureInfo culture = new CultureInfo("en-US");
            Dictionary<string, string> month = new Dictionary<string, string>();
            List<string> Year = new List<string>();
            int i = 0;
            if (_infoBody != null)
            {
                foreach (var item in _incom)
                {
                    if (item.IncomeTime != null && !month.ContainsKey(item.IncomeTime.Value.ToString("yyyy.MM.dd")))
                    {
                        month[item.IncomeTime.Value.ToString("MMMM yyyy", culture)] = item.IncomeTime.Value.ToString("yyyy.MM.dd");

                    }
                    string year = item.IncomeTime.Value.Year.ToString();
                    if (!Year.Contains(year))
                    {
                        Year.Add(year);
                    }
                }
            }
            ViewBag.Month = month;
            ViewBag.Year = Year;

            IEnumerable<IncomePerson> keyValueIncomeInfo = _incom.Where(item => item.IncomeTime.Value.Date == DateTime.Now.Date).GroupBy(item => item.SourceOfIncome)
                .Select(group => new IncomePerson
                {
                    SourceOfIncome = group.Key,
                    Income = group.Sum(item => item.Income),
                    Currency = group.First().Currency,

                }).Distinct();

            ViewBag.DateInfo = DateTime.Now.Date.ToString("dd-MM-yyyy");

            return View(keyValueIncomeInfo);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult InfoIncome(string allTime, string? yearDate = null, DateTime? monthDate = null, DateTime? day = null)
        {
            ViewBag.DateInfo = "";
            CultureInfo culture = new CultureInfo("en-US");
            Dictionary<string, string> month = new Dictionary<string, string>();
            List<string> Year = new List<string>();
            int i = 0;
            if (_incom != null)
            {
                foreach (var item in _incom)
                {
                    if (item.IncomeTime != null && !month.ContainsKey(item.IncomeTime.Value.ToString("yyyy.MM.dd")))
                    {
                        month[item.IncomeTime.Value.ToString("MMMM yyyy", culture)] = item.IncomeTime.Value.ToString("yyyy.MM.dd");

                    }
                    string year = item.IncomeTime.Value.Year.ToString();
                    if (!Year.Contains(year))
                    {
                        Year.Add(year);
                    }
                }
            }
            ViewBag.Month = month;
            ViewBag.Year = Year;

            if (day.HasValue)
            {
                IEnumerable<IncomePerson> dayResult = _incom.Where(item => item.IncomeTime.Value.Date == day.Value.Date).GroupBy(item => item.SourceOfIncome)
                .Select(group => new IncomePerson
                {
                    SourceOfIncome = group.Key,
                    Income = group.Sum(item => item.Income),
                    Currency = group.First().Currency,
                    IncomeTime = group.First().IncomeTime
                }).Distinct();

                if (dayResult == null)
                {
                    dayResult = _incom.GroupBy(item => item.SourceOfIncome)
                        .Select(group => new IncomePerson
                        {
                            SourceOfIncome = "", // Задаємо значення SourceOfIncome
                            Income = 0, // Задаємо значення Income
                            Currency = "" // Задаємо значення Currency
                        });

                }
                ViewBag.DateInfo = day.Value.Date.ToString("dd-MM-yyyy"); ;
                return View(dayResult);


            }


            if (yearDate != null)
            {
                IEnumerable<IncomePerson> AllPeriods = _incom.Where(item => item.IncomeTime.Value.ToString("yyyy") == yearDate).GroupBy(item => item.SourceOfIncome)
                .Select(group => new IncomePerson
                {
                    SourceOfIncome = group.Key,
                    Income = group.Sum(item => item.Income),
                    Currency = group.First().Currency,
                    IncomeTime = group.First().IncomeTime
                }).Distinct();
                ViewBag.DateInfo = yearDate;
                ViewBag.TypeOfPeriod = "year";
                return View(AllPeriods);
            }

            if (monthDate.HasValue)
            {
                IEnumerable<IncomePerson> AllPeriods = _incom.Where(item => item.IncomeTime.Value.ToString("MM") == monthDate.Value.ToString("MM")).GroupBy(item => item.SourceOfIncome)
                .Select(group => new IncomePerson
                {
                    SourceOfIncome = group.Key,
                    Income = group.Sum(item => item.Income),
                    Currency = group.First().Currency,
                    IncomeTime = group.First().IncomeTime
                }).Distinct();
                ViewBag.DateInfo = monthDate.Value.ToString("MMMM yyyy", culture);
                ViewBag.TypeOfPeriod = "month";
                return View(AllPeriods);
            }

            if (allTime != null)
            {
                IEnumerable<IncomePerson> AllPeriods = _incom.GroupBy(item => item.SourceOfIncome)
                .Select(group => new IncomePerson
                {
                    SourceOfIncome = group.Key,
                    Income = group.Sum(item => item.Income),
                    Currency = group.First().Currency,
                    IncomeTime = group.First().IncomeTime
                }).Distinct();
                ViewBag.DateInfo = "All time";
                return View(AllPeriods);
            }

            IEnumerable<IncomePerson> keyValueIncomeInfo = _incom.Where(item => item.IncomeTime.Value.Date == DateTime.Now.Date).GroupBy(item => item.SourceOfIncome)
                .Select(group => new IncomePerson
                {
                    SourceOfIncome = group.Key,
                    Income = group.Sum(item => item.Income),
                    Currency = group.First().Currency,

                }).Distinct();




            return View(keyValueIncomeInfo);


        }


        [Route("[action]")]
        public IActionResult InformationAboutTypeIncome(string TypePeriod, string type, string Period)
        {
            CultureInfo culture = new CultureInfo("en-US");

            List<IncomePerson> incomeInfo = _incom.Where(item => item.SourceOfIncome == type).ToList();
            ViewBag.InfoType = type;
            if (Period == "All time")
            {
                incomeInfo = _incom.Where(item => item.SourceOfIncome == type).ToList();

            }
            else if (TypePeriod == "year")
            {
                incomeInfo = _incom.Where(item => item.SourceOfIncome == type && item.IncomeTime.Value.Date.ToString("yyyy") == Period).ToList();
            }
            else if (TypePeriod == "month")
            {
                incomeInfo = _incom.Where(item => item.SourceOfIncome == type && item.IncomeTime.Value.Date.ToString("MMMM yyyy", culture) == Period).ToList();
            }
			else
			{
                incomeInfo = _incom.Where(item => item.SourceOfIncome == type && item.IncomeTime.Value.Date.ToString("dd-MM-yyyy") == Period).ToList();
            }
           

            ViewBag.DateInfo = Period;
            return View(incomeInfo);
        }


        private void PopulateMonthAndYear()
		{
			CultureInfo culture = new CultureInfo("en-US");

			Dictionary<string, string> month = new Dictionary<string, string>();
			List<string> Year = new List<string>();
			int i = 0;
			if (_infoBody != null)
			{
				foreach (var item in _infoBody)
				{
					if (item.postDateTime != null && !month.ContainsKey(item.postDateTime.Value.ToString("yyyy.MM.dd")))
					{
						month[item.postDateTime.Value.ToString("MMMM yyyy", culture)] = item.postDateTime.Value.ToString("yyyy.MM.dd");

					}
					string year = item.postDateTime.Value.Year.ToString();
					if (!Year.Contains(year))
					{
						Year.Add(year);
					}
				}
			}
			ViewBag.Month = month;
			ViewBag.Year = Year;
		}

		public void CostsAndIncomViewBag(List<IncomePerson> incomePeople, List<InfoBody> infoBodies)
		{
			double? incom = 0.0;
			double? costs = 0.0;
			foreach (var item in incomePeople)
			{
				incom += item.Income;
			}

			foreach (var item in infoBodies)
			{
				costs += item.Price;
			}
			if (incomePeople != null && incomePeople.Any())
			{
				ViewBag.TodayIncom = Math.Round(incom ?? 0.0, 2) + $" {incomePeople[0].Currency}";
			}
			else { ViewBag.TodayIncom = 0.0; }
			
			if (infoBodies !=null && infoBodies.Any())
			{
				ViewBag.TodayCosts = Math.Round(costs ?? 0.0, 2) + $" {infoBodies[0].CurrentyType}";
			}
			else
			{				
				ViewBag.TodayCosts = 0.0;
			}

			ViewBag.Balance = Math.Round(incom - costs ?? 0,2);


		}
		

		public HistoryViewModel InfoToday(string PresentTime)
		{
			HistoryViewModel historyView = new HistoryViewModel()
			{
				incomePersons = _incom.Where(item => item.IncomeTime.Value.ToString("MM-dd-yyyy") == PresentTime).ToList(),
				infoBodies = _infoBody.Where(item => item.postDateTime.Value.ToString("MM-dd-yyyy") == PresentTime).ToList()
			};

			CostsAndIncomViewBag(historyView.incomePersons, historyView.infoBodies);
			ViewBag.InformationAboutCosts = historyView.infoBodies;
			return historyView;
		}

		public static void WriteToJsonFile(HistoryViewModel data, string filePath)
		{
			string jsonString = JsonSerializer.Serialize(data.infoBodies);
			jsonString += JsonSerializer.Serialize(data.incomePersons);
			System.IO.File.WriteAllText(filePath, jsonString);
		}

		public void AverageOfMonth(HistoryViewModel historyView, string year)
		{
			double sumMonth = 0.0;
			int numMonths = 0;
			var test = historyView.infoBodies.OrderBy(item => item.postDateTime.Value.Month);

			Dictionary<int, double> monthlyExpenses = new Dictionary<int, double>();
			foreach (var item in test)
			{
				if (item.postDateTime.Value.Year.ToString() == year)
				{
					int mon = item.postDateTime.Value.Month;


					if (!monthlyExpenses.ContainsKey(mon))
					{
						monthlyExpenses[mon] = item.Price;
					}
					else
					{
						monthlyExpenses[mon] += item.Price;
						numMonths++;
					}

				}
			}


			double totalExpenses = monthlyExpenses.Values.Sum();
			double? averageExpensePerMonth = totalExpenses / (numMonths == 0 ? 2 : numMonths);
			ViewBag.AverageOfMonth = Math.Round(averageExpensePerMonth ?? 0.0, 2);
		}


		public void AveregeOfDay(List<InfoBody> infoBodies, string? StrDay=null)
		{
			if (StrDay == null)
			{
				double sumCostsDay = 0.0;
				int i = 0;
				Dictionary<int, double> dayExpenses = new Dictionary<int, double>();
				foreach (var item in infoBodies)
				{
					int day = item.postDateTime.Value.Day;

					if (!dayExpenses.ContainsKey(day))
					{
						dayExpenses[day] = item.Price;
					}
					else
					{
						dayExpenses[day] += item.Price;
						i++;
					}
				}
				sumCostsDay = Math.Round((dayExpenses.Values.Sum()) / (i == 0 ? 2 : i), 2);
				ViewBag.AverageDay = sumCostsDay;
			}
			else
			{
				ViewBag.AverageDay = 0;
			}
			
		}

		[HttpGet]
		[Route("[action]")]
		public IActionResult Review(string? PresentTime = null)
		{
			PopulateMonthAndYear();

			HistoryViewModel historyView = InfoToday(PresentTime);
			//string filePath = @"C:\Users\shmol\OneDrive\Робочий стіл\Project ASP.NET\ExpenseManager\ExpenseManager\JSONDATA\CostsAndIncomData.json";

			//WriteToJsonFile(historyView, filePath);

			AveregeOfDay(historyView.infoBodies);


			if(historyView.incomePersons.Count == 0 && historyView.infoBodies.Count == 0)
			{
				return RedirectToAction("EmptyPage");
			}

			return View(historyView);
		}




		[HttpPost]
		[Route("[action]")]
		public IActionResult Review(string allTime, string? year=null, DateTime? month = null, DateTime? day = null)
		{				
			PopulateMonthAndYear();
			HistoryViewModel historyView = new HistoryViewModel();

			if (day != null) {
				
				historyView = InfoToday(day.Value.ToString("MM-dd-yyyy"));
				AveregeOfDay(historyView.infoBodies, day.Value.Day.ToString());
				return View(historyView);
			}

			if (month.HasValue)
			{
				historyView = new HistoryViewModel()
				{
					incomePersons = _incom.Where(item => item.IncomeTime.Value.Date.Month == month.Value.Month).ToList(),
					infoBodies = _infoBody.Where(item => item.postDateTime?.Date.Month == month.Value.Month).ToList()
				};
				ViewBag.InformationAboutCosts = historyView.infoBodies;
				CostsAndIncomViewBag(historyView.incomePersons, historyView.infoBodies);
				AveregeOfDay(historyView.infoBodies);
				return View(historyView);
			}

			

			if (year!=null)
			{
				historyView = new HistoryViewModel()
				{
					incomePersons = _incom.Where(item => item.IncomeTime.Value.Date.Year.ToString() == year).ToList(),
					infoBodies = _infoBody.Where(item => item.postDateTime?.Date.Year.ToString() == year).ToList()
				};
				ViewBag.InformationAboutCosts = historyView.infoBodies;
				CostsAndIncomViewBag(historyView.incomePersons, historyView.infoBodies);

				AverageOfMonth(historyView, year);
				AveregeOfDay(historyView.infoBodies);
				return View(historyView);
			}
			if (allTime!=null)
			{
				historyView = new HistoryViewModel()
				{
					incomePersons = _incom,
					infoBodies = _infoBody
                };
                ViewBag.InformationAboutCosts = historyView.infoBodies;
                CostsAndIncomViewBag(historyView.incomePersons, historyView.infoBodies);

                AverageOfMonth(historyView, year);
                AveregeOfDay(historyView.infoBodies);
                return View(historyView);
            }
			

			return View();
		}

		[HttpPost]
		[Route("[action]")]
		public IActionResult EditInformation(int Id,string body, string expenditureType, double price, string currentyType)
		{
			int? index = _incom.FindIndex(item => item.Id == Id);
			if (index != -1)
			{
				_incom[index.Value].Income = price;
				_incom[index.Value].Currency = currentyType;
				_incom[index.Value].SourceOfIncome = expenditureType;
			}
			int? indexCosts = _infoBody.FindIndex(item => item.Id == Id);
			if(indexCosts !=-1)
            {
				_infoBody[indexCosts.Value].Body = body;
                _infoBody[indexCosts.Value].Price = price;
                _infoBody[indexCosts.Value].CurrentyType = currentyType;
                _infoBody[indexCosts.Value].ExpenditureType = expenditureType;
            }

            

			return RedirectToAction("History");
		}


	}
}
