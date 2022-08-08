// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using (var context = new CustomerContext())
{
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    context.AddRange(
        new Customer
        {
            Name = "Alice",
            PhoneNumber = "515 555 0123",
            Country = context.Countries.Find("United States")!
        },
        new Customer
        {
            Name = "Mac",
            PhoneNumber = "515 555 0124",
            Country = context.Countries.Find("United Kingdom")!
        });

    context.SaveChanges();
}

using (var context = new CustomerContext())
{
    foreach (var customer in context.Customers.Include(e => e.Country))
    {
        Console.WriteLine($"Customer '{customer.Name}' in Country '{customer.Country.Name}'");
    }
}

public class CustomerContext : DbContext
{
    private static readonly CountriesCachingInterceptor _countriesCachingInterceptor = new();

    public DbSet<Customer> Customers
        => Set<Customer>();

    public DbSet<Country> Countries
        => Set<Country>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .AddInterceptors(_countriesCachingInterceptor)
            .UseSqlite("Data Source = customers.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.Entity<Country>().HasData(
            new Country("Afghanistan", "+93"),
            new Country("Åland", "+358 18"),
            new Country("Albania", "+355"),
            new Country("Algeria", "+213"),
            new Country("American Samoa", "+1 684"),
            new Country("Andorra", "+376"),
            new Country("Angola", "+244"),
            new Country("Anguilla", "+1 264"),
            new Country("Antigua and Barbuda", "+1 268"),
            new Country("Argentina", "+54"),
            new Country("Armenia", "+374"),
            new Country("Aruba", "+297"),
            new Country("Ascension", "+247"),
            new Country("Australia", "+61"),
            new Country("Australian Antarctic Territory", "+672 1"),
            new Country("Australian External Territories", "+672"),
            new Country("Austria", "+43"),
            new Country("Azerbaijan", "+994"),
            new Country("Bahamas", "+1 242"),
            new Country("Bahrain", "+973"),
            new Country("Bangladesh", "+880"),
            new Country("Barbados", "+1 246"),
            new Country("Barbuda", "+1 268"),
            new Country("Belarus", "+375"),
            new Country("Belgium", "+32"),
            new Country("Belize", "+501"),
            new Country("Benin", "+229"),
            new Country("Bermuda", "+1 441"),
            new Country("Bhutan", "+975"),
            new Country("Bolivia", "+591"),
            new Country("Bonaire", "+599 7"),
            new Country("Bosnia and Herzegovina", "+387"),
            new Country("Botswana", "+267"),
            new Country("Brazil", "+55"),
            new Country("British Indian Ocean Territory", "+246"),
            new Country("British Virgin Islands", "+1 284"),
            new Country("Brunei Darussalam", "+673"),
            new Country("Bulgaria", "+359"),
            new Country("Burkina Faso", "+226"),
            new Country("Burundi", "+257"),
            new Country("Cape Verde", "+238"),
            new Country("Cambodia", "+855"),
            new Country("Cameroon", "+237"),
            new Country("Canada", "+1"),
            new Country("Caribbean Netherlands", "+599 3, +599 4, +599 7"),
            new Country("Cayman Islands", "+1 345"),
            new Country("Central African Republic", "+236"),
            new Country("Chad", "+235"),
            new Country("Chatham Island, New Zealand", "+64"),
            new Country("Chile", "+56"),
            new Country("China", "+86"),
            new Country("Christmas Island", "+61 89164"),
            new Country("Cocos (Keeling) Islands", "+61 89162"),
            new Country("Colombia", "+57"),
            new Country("Comoros", "+269"),
            new Country("Congo", "+242"),
            new Country("Congo, Democratic Republic of the", "+243"),
            new Country("Cook Islands", "+682"),
            new Country("Costa Rica", "+506"),
            new Country("Ivory Coast (Côte d'Ivoire)", "+225"),
            new Country("Croatia", "+385"),
            new Country("Cuba", "+53"),
            new Country("Curaçao", "+599 9"),
            new Country("Cyprus", "+357"),
            new Country("Czech Republic", "+420"),
            new Country("Denmark", "+45"),
            new Country("Diego Garcia", "+246"),
            new Country("Djibouti", "+253"),
            new Country("Dominica", "+1 767"),
            new Country("Dominican Republic", "+1 809, +1 829,"),
            new Country("Easter Island", "+56"),
            new Country("Ecuador", "+593"),
            new Country("Egypt", "+20"),
            new Country("El Salvador", "+503"),
            new Country("Equatorial Guinea", "+240"),
            new Country("Eritrea", "+291"),
            new Country("Estonia", "+372"),
            new Country("Eswatini", "+268"),
            new Country("Ethiopia", "+251"),
            new Country("Falkland Islands", "+500"),
            new Country("Faroe Islands", "+298"),
            new Country("Fiji", "+679"),
            new Country("Finland", "+358"),
            new Country("France", "+33"),
            new Country("French Antilles", "+596"),
            new Country("French Guiana", "+594"),
            new Country("French Polynesia", "+689"),
            new Country("Gabon", "+241"),
            new Country("Gambia", "+220"),
            new Country("Georgia", "+995"),
            new Country("Germany", "+49"),
            new Country("Ghana", "+233"),
            new Country("Gibraltar", "+350"),
            new Country("Greece", "+30"),
            new Country("Greenland", "+299"),
            new Country("Grenada", "+1 473"),
            new Country("Guadeloupe", "+590"),
            new Country("Guam", "+1 671"),
            new Country("Guatemala", "+502"),
            new Country("Guernsey", "+44 1481, +44 7781,"),
            new Country("Guinea", "+224"),
            new Country("Guinea-Bissau", "+245"),
            new Country("Guyana", "+592"),
            new Country("Haiti", "+509"),
            new Country("Honduras", "+504"),
            new Country("Hong Kong", "+852"),
            new Country("Hungary", "+36"),
            new Country("Iceland", "+354"),
            new Country("India", "+91"),
            new Country("Indonesia", "+62"),
            new Country("Iran", "+98"),
            new Country("Iraq", "+964"),
            new Country("Ireland", "+353"),
            new Country("Isle of Man", "+44 1624, +44 7524,"),
            new Country("Israel", "+972"),
            new Country("Italy", "+39"),
            new Country("Jamaica", "+1 658, +1 876"),
            new Country("Jan Mayen", "+47 79"),
            new Country("Japan", "+81"),
            new Country("Jersey", "+44 1534"),
            new Country("Jordan", "+962"),
            new Country("Kazakhstan", "+7 6, +7 7[notes 1]"),
            new Country("Kenya", "+254"),
            new Country("Kiribati", "+686"),
            new Country("Korea, North", "+850"),
            new Country("Korea, South", "+82"),
            new Country("Kosovo", "+383"),
            new Country("Kuwait", "+965"),
            new Country("Kyrgyzstan", "+996"),
            new Country("Laos", "+856"),
            new Country("Latvia", "+371"),
            new Country("Lebanon", "+961"),
            new Country("Lesotho", "+266"),
            new Country("Liberia", "+231"),
            new Country("Libya", "+218"),
            new Country("Liechtenstein", "+423"),
            new Country("Lithuania", "+370"),
            new Country("Luxembourg", "+352"),
            new Country("Macau", "+853"),
            new Country("Madagascar", "+261"),
            new Country("Malawi", "+265"),
            new Country("Malaysia", "+60"),
            new Country("Maldives", "+960"),
            new Country("Mali", "+223"),
            new Country("Malta", "+356"),
            new Country("Marshall Islands", "+692"),
            new Country("Martinique", "+596"),
            new Country("Mauritania", "+222"),
            new Country("Mauritius", "+230"),
            new Country("Mayotte", "+262 269, +262 639"),
            new Country("Mexico", "+52"),
            new Country("Micronesia, Federated States of", "+691"),
            new Country("Midway Island, USA", "+1 808"),
            new Country("Moldova", "+373"),
            new Country("Monaco", "+377"),
            new Country("Mongolia", "+976"),
            new Country("Montenegro", "+382"),
            new Country("Montserrat", "+1 664"),
            new Country("Morocco", "+212"),
            new Country("Mozambique", "+258"),
            new Country("Myanmar", "+95"),
            new Country("Artsakh", "+374 47, +374 97"),
            new Country("Namibia", "+264"),
            new Country("Nauru", "+674"),
            new Country("Nepal", "+977"),
            new Country("Netherlands", "+31"),
            new Country("Nevis", "+1 869"),
            new Country("New Caledonia", "+687"),
            new Country("New Zealand", "+64"),
            new Country("Nicaragua", "+505"),
            new Country("Niger", "+227"),
            new Country("Nigeria", "+234"),
            new Country("Niue", "+683"),
            new Country("Norfolk Island", "+672 3"),
            new Country("North Macedonia", "+389"),
            new Country("Northern Cyprus", "+90 392"),
            new Country("Northern Ireland", "+44 28"),
            new Country("Northern Mariana Islands", "+1 670"),
            new Country("Norway", "+47"),
            new Country("Oman", "+968"),
            new Country("Pakistan", "+92"),
            new Country("Palau", "+680"),
            new Country("Palestine, State of", "+970"),
            new Country("Panama", "+507"),
            new Country("Papua New Guinea", "+675"),
            new Country("Paraguay", "+595"),
            new Country("Peru", "+51"),
            new Country("Philippines", "+63"),
            new Country("Pitcairn Islands", "+64"),
            new Country("Poland", "+48"),
            new Country("Portugal", "+351"),
            new Country("Puerto Rico", "+1 787, +1 939"),
            new Country("Qatar", "+974"),
            new Country("Réunion", "+262"),
            new Country("Romania", "+40"),
            new Country("Russia", "+7[notes 1]"),
            new Country("Rwanda", "+250"),
            new Country("Saba", "+599 4"),
            new Country("Saint Barthélemy", "+590"),
            new Country("Saint Helena", "+290"),
            new Country("Saint Kitts and Nevis", "+1 869"),
            new Country("Saint Lucia", "+1 758"),
            new Country("Saint Martin (France)", "+590"),
            new Country("Saint Pierre and Miquelon", "+508"),
            new Country("Saint Vincent and the Grenadines", "+1 784"),
            new Country("Samoa", "+685"),
            new Country("San Marino", "+378"),
            new Country("São Tomé and Príncipe", "+239"),
            new Country("Saudi Arabia", "+966"),
            new Country("Senegal", "+221"),
            new Country("Serbia", "+381"),
            new Country("Seychelles", "+248"),
            new Country("Sierra Leone", "+232"),
            new Country("Singapore", "+65"),
            new Country("Sint Eustatius", "+599 3"),
            new Country("Sint Maarten (Netherlands)", "+1 721"),
            new Country("Slovakia", "+421"),
            new Country("Slovenia", "+386"),
            new Country("Solomon Islands", "+677"),
            new Country("Somalia", "+252"),
            new Country("South Africa", "+27"),
            new Country("South Georgia and the South Sandwich Islands", "+500"),
            new Country("South Ossetia", "+995 34"),
            new Country("South Sudan", "+211"),
            new Country("Spain", "+34"),
            new Country("Sri Lanka", "+94"),
            new Country("Sudan", "+249"),
            new Country("Suriname", "+597"),
            new Country("Svalbard", "+47 79"),
            new Country("Sweden", "+46"),
            new Country("Switzerland", "+41"),
            new Country("Syria", "+963"),
            new Country("Taiwan", "+886"),
            new Country("Tajikistan", "+992"),
            new Country("Tanzania", "+255"),
            new Country("Thailand", "+66"),
            new Country("Thuraya (Mobile Satellite service)", "+882 16"),
            new Country("East Timor (Timor-Leste)", "+670"),
            new Country("Togo", "+228"),
            new Country("Tokelau", "+690"),
            new Country("Tonga", "+676"),
            new Country("Transnistria", "+373 2, +373 5"),
            new Country("Trinidad and Tobago", "+1 868"),
            new Country("Tristan da Cunha", "+290 8"),
            new Country("Tunisia", "+216"),
            new Country("Turkey", "+90"),
            new Country("Turkmenistan", "+993"),
            new Country("Turks and Caicos Islands", "+1 649"),
            new Country("Tuvalu", "+688"),
            new Country("Uganda", "+256"),
            new Country("Ukraine", "+380"),
            new Country("United Arab Emirates", "+971"),
            new Country("United Kingdom", "+44"),
            new Country("United States", "+1"),
            new Country("Universal Personal Telecommunications (UPT)", "+878"),
            new Country("Uruguay", "+598"),
            new Country("US Virgin Islands", "+1 340"),
            new Country("Uzbekistan", "+998"),
            new Country("Vanuatu", "+678"),
            new Country("Vatican City State (Holy See)", "+39 06 698,"),
            new Country("Venezuela", "+58"),
            new Country("Vietnam", "+84"),
            new Country("Wake Island, USA", "+1 808"),
            new Country("Wallis and Futuna", "+681"),
            new Country("Yemen", "+967"),
            new Country("Zambia", "+260"),
            new Country("Zanzibar", "+255 24"),
            new Country("Zimbabwe", "+263"));
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public Country Country { get; set; } = null!;
}

public sealed class Country
{
    public Country(string name, string dialingCodes)
    {
        Name = name;
        DialingCodes = dialingCodes;
    }

    [Key]
    public string Name { get; private set; }

    public string DialingCodes { get; }
}

public class CountriesCachingInterceptor : IMaterializationInterceptor
{
    private static readonly ConcurrentDictionary<string, Country> ToppingsCache = new();

    public InterceptionResult<object> CreatingInstance(
        MaterializationInterceptionData materializationData,
        InterceptionResult<object> result)
    {
        if (materializationData.EntityType.ClrType == typeof(Country))
        {
            var countryName = materializationData.GetPropertyValue<string>(nameof(Country.Name));
            if (ToppingsCache.TryGetValue(countryName, out var country))
            {
                Console.WriteLine($"Got country '{country.Name}' from cache.");
                return InterceptionResult<object>.SuppressWithResult(country);
            }
        }

        return result;
    }

    public object CreatedInstance(MaterializationInterceptionData materializationData, object instance)
    {
        if (instance is Country country
            && ToppingsCache.TryAdd(country.Name, country))
        {
            Console.WriteLine($"Country '{country.Name}' added to cache.");
        }

        return instance;
    }
}
