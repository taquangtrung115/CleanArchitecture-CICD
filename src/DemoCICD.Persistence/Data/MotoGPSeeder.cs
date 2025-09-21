using DemoCICD.Domain.Entities.MotoGP.RaceManagement;
using DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;
using DemoCICD.Domain.Entities.MotoGP.MediaNews;
using DemoCICD.Domain.Entities.MotoGP.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Data;

public static class MotoGPSeeder
{
    public static async Task SeedMotoGPDataAsync(ApplicationDbContext context)
    {
        await SeedSeasonsAsync(context);
        await SeedTeamsAsync(context);
        await SeedRidersAsync(context);
        await SeedBikesAsync(context);
        await SeedRacesAsync(context);
        await SeedRaceEntriesAsync(context);
        await SeedRiderTeamHistoryAsync(context);
        await SeedVideosAsync(context);
        
        await context.SaveChangesAsync();
    }

    private static async Task SeedSeasonsAsync(ApplicationDbContext context)
    {
        if (await context.Seasons.AnyAsync())
            return; // Data already exists

        var seasons = new List<Season>
        {
            Season.Create(2024, "2024 MotoGP World Championship", new DateTime(2024, 3, 1), new DateTime(2024, 11, 30), "The 76th FIM Road Racing World Championship Grand Prix season"),
            Season.Create(2023, "2023 MotoGP World Championship", new DateTime(2023, 3, 1), new DateTime(2023, 11, 30), "The 75th FIM Road Racing World Championship Grand Prix season"),
            Season.Create(2022, "2022 MotoGP World Championship", new DateTime(2022, 3, 1), new DateTime(2022, 11, 30), "The 74th FIM Road Racing World Championship Grand Prix season"),
            Season.Create(2021, "2021 MotoGP World Championship", new DateTime(2021, 3, 1), new DateTime(2021, 11, 30), "The 73rd FIM Road Racing World Championship Grand Prix season"),
            Season.Create(2020, "2020 MotoGP World Championship", new DateTime(2020, 3, 1), new DateTime(2020, 11, 30), "The 72nd FIM Road Racing World Championship Grand Prix season - COVID-19 affected"),
            Season.Create(2019, "2019 MotoGP World Championship", new DateTime(2019, 3, 1), new DateTime(2019, 11, 30), "The 71st FIM Road Racing World Championship Grand Prix season"),
            Season.Create(2018, "2018 MotoGP World Championship", new DateTime(2018, 3, 1), new DateTime(2018, 11, 30), "The 70th FIM Road Racing World Championship Grand Prix season"),
            Season.Create(2017, "2017 MotoGP World Championship", new DateTime(2017, 3, 1), new DateTime(2017, 11, 30), "The 69th FIM Road Racing World Championship Grand Prix season")
        };

        // Set 2024 as current season
        seasons[0].SetAsCurrentSeason();
        // Set completed status for past seasons
        for (int i = 1; i < seasons.Count; i++)
        {
            seasons[i].CompleteSeason();
        }

        context.Seasons.AddRange(seasons);
    }

    private static async Task SeedTeamsAsync(ApplicationDbContext context)
    {
        if (await context.Teams.AnyAsync())
            return; // Data already exists

        var teams = new List<Team>
        {
            Team.Create("Ducati Lenovo Team", "Ducati", new Country("ITA", "Italy", "🇮🇹"), new DateTime(1926, 7, 4), "Official Ducati factory team"),
            Team.Create("Red Bull KTM Factory Racing", "KTM", new Country("AUT", "Austria", "🇦🇹"), new DateTime(1992, 1, 1), "KTM factory racing team"),
            Team.Create("Monster Energy Yamaha MotoGP", "Yamaha", new Country("JPN", "Japan", "🇯🇵"), new DateTime(1961, 1, 1), "Official Yamaha factory team"),
            Team.Create("Repsol Honda Team", "Honda", new Country("JPN", "Japan", "🇯🇵"), new DateTime(1982, 1, 1), "Official Honda factory team"),
            Team.Create("Prima Pramac Racing", "Pramac", new Country("ITA", "Italy", "🇮🇹"), new DateTime(2005, 1, 1), "Independent Ducati team"),
            Team.Create("Aprilia Racing", "Aprilia", new Country("ITA", "Italy", "🇮🇹"), new DateTime(2015, 1, 1), "Official Aprilia factory team"),
            Team.Create("Gresini Racing MotoGP", "Gresini", new Country("ITA", "Italy", "🇮🇹"), new DateTime(1997, 1, 1), "Independent Ducati team"),
            Team.Create("VR46 Racing Team", "VR46", new Country("ITA", "Italy", "🇮🇹"), new DateTime(2021, 1, 1), "Valentino Rossi's team"),
            Team.Create("LCR Honda", "LCR", new Country("GBR", "United Kingdom", "🇬🇧"), new DateTime(1996, 1, 1), "Independent Honda team"),
            Team.Create("RNF MotoGP Team", "RNF", new Country("MYS", "Malaysia", "🇲🇾"), new DateTime(2021, 1, 1), "Malaysian satellite team")
        };

        context.Teams.AddRange(teams);
    }

    private static async Task SeedRidersAsync(ApplicationDbContext context)
    {
        if (await context.Riders.AnyAsync())
            return; // Data already exists

        var riders = new List<Rider>
        {
            Rider.Create("Francesco", "Bagnaia", 1, new Country("ITA", "Italy", "🇮🇹"), new DateTime(1997, 1, 14), 175m, 67m, "Pecco"),
            Rider.Create("Jorge", "Martin", 89, new Country("ESP", "Spain", "🇪🇸"), new DateTime(1998, 1, 29), 173m, 66m, "Martinator"),
            Rider.Create("Marc", "Marquez", 93, new Country("ESP", "Spain", "🇪🇸"), new DateTime(1993, 2, 17), 168m, 59m, "The Ant of Cervera"),
            Rider.Create("Fabio", "Quartararo", 20, new Country("FRA", "France", "🇫🇷"), new DateTime(1999, 4, 20), 175m, 67m, "El Diablo"),
            Rider.Create("Jack", "Miller", 43, new Country("AUS", "Australia", "🇦🇺"), new DateTime(1995, 1, 18), 182m, 71m, "Jackass"),
            Rider.Create("Brad", "Binder", 33, new Country("ZAF", "South Africa", "🇿🇦"), new DateTime(1995, 8, 11), 169m, 64m, "The Bulldog"),
            Rider.Create("Aleix", "Espargaro", 41, new Country("ESP", "Spain", "🇪🇸"), new DateTime(1989, 7, 30), 171m, 64m, "The Captain"),
            Rider.Create("Enea", "Bastianini", 23, new Country("ITA", "Italy", "🇮🇹"), new DateTime(1997, 12, 30), 183m, 72m, "The Beast"),
            Rider.Create("Maverick", "Vinales", 12, new Country("ESP", "Spain", "🇪🇸"), new DateTime(1995, 1, 12), 171m, 64m, "Top Gun"),
            Rider.Create("Pedro", "Acosta", 31, new Country("ESP", "Spain", "🇪🇸"), new DateTime(2004, 5, 25), 168m, 60m, "The Shark")
        };

        context.Riders.AddRange(riders);
    }

    private static async Task SeedBikesAsync(ApplicationDbContext context)
    {
        if (await context.Bikes.AnyAsync())
            return; // Data already exists

        var bikes = new List<Bike>
        {
            Bike.Create("Ducati", "Desmosedici GP24", 2024, 
                new BikeSpec("Desmosedici 90° V4", 1000, 270, 356m, 157m, "6-speed", "Carbon fiber", "Öhlins USD", "Öhlins", "Brembo", "Brembo"),
                "DUCATI001", "DUC2024001"),
            Bike.Create("Yamaha", "YZR-M1", 2024,
                new BikeSpec("Inline-4", 1000, 240, 340m, 157m, "6-speed", "Aluminum", "KYB USD", "KYB", "Brembo", "Brembo"),
                "YAMAHA001", "YAM2024001"),
            Bike.Create("Honda", "RC213V", 2024,
                new BikeSpec("V4", 1000, 260, 345m, 157m, "6-speed", "Carbon fiber", "Showa USD", "Showa", "Brembo", "Brembo"),
                "HONDA001", "HON2024001"),
            Bike.Create("KTM", "RC16", 2024,
                new BikeSpec("V4", 1000, 265, 350m, 157m, "6-speed", "Steel trellis", "WP USD", "WP", "Brembo", "Brembo"),
                "KTM001", "KTM2024001"),
            Bike.Create("Aprilia", "RS-GP", 2024,
                new BikeSpec("V4", 1000, 250, 342m, 157m, "6-speed", "Aluminum", "Öhlins USD", "Öhlins", "Brembo", "Brembo"),
                "APRILIA001", "APR2024001"),
            Bike.Create("Ducati", "Desmosedici GP23", 2023,
                new BikeSpec("Desmosedici 90° V4", 1000, 265, 354m, 157m, "6-speed", "Carbon fiber", "Öhlins USD", "Öhlins", "Brembo", "Brembo"),
                "DUCATI002", "DUC2023001"),
            Bike.Create("Yamaha", "YZR-M1", 2023,
                new BikeSpec("Inline-4", 1000, 235, 338m, 157m, "6-speed", "Aluminum", "KYB USD", "KYB", "Brembo", "Brembo"),
                "YAMAHA002", "YAM2023001"),
            Bike.Create("Honda", "RC213V", 2023,
                new BikeSpec("V4", 1000, 255, 343m, 157m, "6-speed", "Carbon fiber", "Showa USD", "Showa", "Brembo", "Brembo"),
                "HONDA002", "HON2023001"),
            Bike.Create("KTM", "RC16", 2023,
                new BikeSpec("V4", 1000, 260, 348m, 157m, "6-speed", "Steel trellis", "WP USD", "WP", "Brembo", "Brembo"),
                "KTM002", "KTM2023001"),
            Bike.Create("Aprilia", "RS-GP", 2023,
                new BikeSpec("V4", 1000, 245, 340m, 157m, "6-speed", "Aluminum", "Öhlins USD", "Öhlins", "Brembo", "Brembo"),
                "APRILIA002", "APR2023001")
        };

        context.Bikes.AddRange(bikes);
    }

    private static async Task SeedRacesAsync(ApplicationDbContext context)
    {
        if (await context.Races.AnyAsync())
            return; // Data already exists

        var season2024 = await context.Seasons.FirstOrDefaultAsync(s => s.Year == 2024);
        if (season2024 == null) return;

        var baseDate = new DateTime(2024, 3, 10);
        var races = new List<Race>
        {
            Race.Create(season2024.Id, "Qatar Grand Prix", "Losail International Circuit", new Country("QAT", "Qatar", "🇶🇦"),
                baseDate, new RaceSchedule(baseDate.AddDays(-2), baseDate.AddDays(-1), baseDate, baseDate.AddHours(3)), 1, 5.380m, 22, "Season opener under the lights"),
            
            Race.Create(season2024.Id, "Indonesian Grand Prix", "Mandalika International Street Circuit", new Country("IDN", "Indonesia", "🇮🇩"),
                baseDate.AddDays(14), new RaceSchedule(baseDate.AddDays(12), baseDate.AddDays(13), baseDate.AddDays(14), baseDate.AddDays(14).AddHours(3)), 2, 4.318m, 27, "Tropical racing challenge"),
            
            Race.Create(season2024.Id, "Argentine Grand Prix", "Termas de Río Hondo", new Country("ARG", "Argentina", "🇦🇷"),
                baseDate.AddDays(28), new RaceSchedule(baseDate.AddDays(26), baseDate.AddDays(27), baseDate.AddDays(28), baseDate.AddDays(28).AddHours(3)), 3, 4.806m, 25, "South American adventure"),
            
            Race.Create(season2024.Id, "Spanish Grand Prix", "Circuito de Jerez", new Country("ESP", "Spain", "🇪🇸"),
                baseDate.AddDays(42), new RaceSchedule(baseDate.AddDays(40), baseDate.AddDays(41), baseDate.AddDays(42), baseDate.AddDays(42).AddHours(3)), 4, 4.423m, 25, "Home race for Spanish riders"),
            
            Race.Create(season2024.Id, "French Grand Prix", "Le Mans", new Country("FRA", "France", "🇫🇷"),
                baseDate.AddDays(56), new RaceSchedule(baseDate.AddDays(54), baseDate.AddDays(55), baseDate.AddDays(56), baseDate.AddDays(56).AddHours(3)), 5, 4.185m, 28, "Historic French circuit"),
            
            Race.Create(season2024.Id, "Italian Grand Prix", "Mugello", new Country("ITA", "Italy", "🇮🇹"),
                baseDate.AddDays(70), new RaceSchedule(baseDate.AddDays(68), baseDate.AddDays(69), baseDate.AddDays(70), baseDate.AddDays(70).AddHours(3)), 6, 5.245m, 23, "The cathedral of speed"),
            
            Race.Create(season2024.Id, "Catalan Grand Prix", "Circuit de Barcelona-Catalunya", new Country("ESP", "Spain", "🇪🇸"),
                baseDate.AddDays(84), new RaceSchedule(baseDate.AddDays(82), baseDate.AddDays(83), baseDate.AddDays(84), baseDate.AddDays(84).AddHours(3)), 7, 4.655m, 24, "Technical Spanish circuit"),
            
            Race.Create(season2024.Id, "German Grand Prix", "Sachsenring", new Country("DEU", "Germany", "🇩🇪"),
                baseDate.AddDays(98), new RaceSchedule(baseDate.AddDays(96), baseDate.AddDays(97), baseDate.AddDays(98), baseDate.AddDays(98).AddHours(3)), 8, 3.671m, 30, "Compact and challenging"),
            
            Race.Create(season2024.Id, "Dutch Grand Prix", "TT Circuit Assen", new Country("NLD", "Netherlands", "🇳🇱"),
                baseDate.AddDays(112), new RaceSchedule(baseDate.AddDays(110), baseDate.AddDays(111), baseDate.AddDays(112), baseDate.AddDays(112).AddHours(3)), 9, 4.542m, 26, "The cathedral of motorcycle racing"),
            
            Race.Create(season2024.Id, "Finnish Grand Prix", "KymiRing", new Country("FIN", "Finland", "🇫🇮"),
                baseDate.AddDays(126), new RaceSchedule(baseDate.AddDays(124), baseDate.AddDays(125), baseDate.AddDays(126), baseDate.AddDays(126).AddHours(3)), 10, 4.560m, 24, "Return to Finland")
        };

        context.Races.AddRange(races);
    }

    private static async Task SeedRaceEntriesAsync(ApplicationDbContext context)
    {
        if (await context.RaceEntries.AnyAsync())
            return; // Data already exists

        var races = await context.Races.ToListAsync();
        var riders = await context.Riders.ToListAsync();
        var teams = await context.Teams.ToListAsync();
        var bikes = await context.Bikes.ToListAsync();

        if (!races.Any() || !riders.Any() || !teams.Any() || !bikes.Any()) return;

        var raceEntries = new List<RaceEntry>();

        foreach (var race in races.Take(3)) // Only for first 3 races
        {
            for (int i = 0; i < Math.Min(8, riders.Count); i++)
            {
                var rider = riders[i];
                var team = teams[i % teams.Count];
                var bike = bikes[i % bikes.Count];

                var entry = new RaceEntry(
                    Guid.NewGuid(),
                    race.Id,
                    rider.Id,
                    team.Id,
                    bike.Id,
                    i + 1, // Starting grid
                    $"Entry for {rider.FullName}"
                );

                entry.SetCreatedAudit("System");
                raceEntries.Add(entry);
            }
        }

        context.RaceEntries.AddRange(raceEntries);
    }

    private static async Task SeedRiderTeamHistoryAsync(ApplicationDbContext context)
    {
        if (await context.RiderTeamHistories.AnyAsync())
            return; // Data already exists

        var riders = await context.Riders.ToListAsync();
        var teams = await context.Teams.ToListAsync();
        var seasons = await context.Seasons.ToListAsync();

        if (!riders.Any() || !teams.Any() || !seasons.Any()) return;

        var histories = new List<RiderTeamHistory>();

        for (int i = 0; i < Math.Min(8, riders.Count); i++)
        {
            var rider = riders[i];
            var team = teams[i % teams.Count];
            var season = seasons.FirstOrDefault(s => s.Year == 2024);

            if (season != null)
            {
                var history = new RiderTeamHistory(
                    Guid.NewGuid(),
                    rider.Id,
                    team.Id,
                    season.Id,
                    new DateTime(2024, 1, 1)
                );

                history.SetCreatedAudit("System");
                histories.Add(history);
            }
        }

        context.RiderTeamHistories.AddRange(histories);
    }

    private static async Task SeedVideosAsync(ApplicationDbContext context)
    {
        if (await context.Videos.AnyAsync())
            return; // Data already exists

        var videos = new List<Video>
        {
            new Video(
                Guid.NewGuid(),
                "MotoGP 2024 Season Preview",
                "Complete preview of the upcoming 2024 MotoGP season with rider interviews and bike presentations",
                VideoType.Documentary,
                "https://video.motogp.com/2024-season-preview",
                TimeSpan.FromMinutes(45),
                "MotoGP.com"
            ),
            new Video(
                Guid.NewGuid(),
                "Best Overtakes 2023",
                "The most spectacular overtaking maneuvers from the 2023 MotoGP season",
                VideoType.Highlight,
                "https://video.motogp.com/best-overtakes-2023",
                TimeSpan.FromMinutes(15),
                "MotoGP.com"
            ),
            new Video(
                Guid.NewGuid(),
                "Behind the Scenes: Ducati Factory",
                "Exclusive look inside the Ducati factory where the championship-winning bikes are built",
                VideoType.Documentary,
                "https://video.motogp.com/ducati-factory",
                TimeSpan.FromMinutes(30),
                "MotoGP.com"
            ),
            new Video(
                Guid.NewGuid(),
                "Valentino Rossi Tribute",
                "A tribute to the legendary nine-time world champion's incredible career",
                VideoType.Documentary,
                "https://video.motogp.com/rossi-tribute",
                TimeSpan.FromMinutes(60),
                "MotoGP.com"
            ),
            new Video(
                Guid.NewGuid(),
                "Qatar GP 2024 Race Highlights",
                "Extended highlights from the season-opening Qatar Grand Prix",
                VideoType.Highlight,
                "https://video.motogp.com/qatar-2024-highlights",
                TimeSpan.FromMinutes(20),
                "MotoGP.com"
            ),
            new Video(
                Guid.NewGuid(),
                "Rookie Report: Pedro Acosta",
                "Following the young Spanish sensation in his debut MotoGP season",
                VideoType.Interview,
                "https://video.motogp.com/acosta-rookie",
                TimeSpan.FromMinutes(25),
                "MotoGP.com"
            ),
            new Video(
                Guid.NewGuid(),
                "Technology Focus: Electronics",
                "Deep dive into the sophisticated electronics systems used in modern MotoGP",
                VideoType.Analysis,
                "https://video.motogp.com/electronics-tech",
                TimeSpan.FromMinutes(35),
                "MotoGP.com"
            ),
            new Video(
                Guid.NewGuid(),
                "Classic Races: Laguna Seca 2008",
                "Relive the epic battle between Valentino Rossi and Casey Stoner",
                VideoType.Highlight,
                "https://video.motogp.com/laguna-seca-2008",
                TimeSpan.FromMinutes(50),
                "MotoGP.com"
            ),
            new Video(
                Guid.NewGuid(),
                "Safety in MotoGP",
                "Comprehensive look at safety measures and innovations in Grand Prix motorcycle racing",
                VideoType.Documentary,
                "https://video.motogp.com/safety-motogp",
                TimeSpan.FromMinutes(40),
                "MotoGP.com"
            ),
            new Video(
                Guid.NewGuid(),
                "MotoGP Legends Panel",
                "Panel discussion with former world champions sharing their experiences",
                VideoType.Interview,
                "https://video.motogp.com/legends-panel",
                TimeSpan.FromMinutes(90),
                "MotoGP.com"
            )
        };

        // Set them as published and add some audit info
        foreach (var video in videos)
        {
            video.Publish();
            video.SetCreatedAudit("System");
        }

        context.Videos.AddRange(videos);
    }
}