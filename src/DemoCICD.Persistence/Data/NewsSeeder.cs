using DemoCICD.Domain.Entities.MotoGP.MediaNews;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Data;

public static class NewsSeeder
{
    public static async Task SeedNewsAsync(ApplicationDbContext context)
    {
        if (await context.Set<News>().AnyAsync())
            return; // Data already exists

        var sampleAuthorId = Guid.NewGuid();
        var sampleNews = new List<News>
        {
            News.Create(
                "Bagnaia wins dramatic season finale at Valencia",
                "Francesco Bagnaia secured a thrilling victory in the final race of the MotoGP season at Valencia, finishing ahead of championship rival Jorge Martin in a spectacular showdown.",
                "In an unforgettable season finale at the Circuit Ricardo Tormo, Francesco Bagnaia delivered a masterclass performance to claim victory in Valencia. The Italian rider, starting from pole position, managed to hold off intense pressure from championship leader Jorge Martin throughout the 27-lap race.\n\nThe race was marked by several dramatic moments, including a close battle between the title contenders in the early stages. Bagnaia's perfect race craft and strategic tire management proved decisive as he gradually built a gap to secure his fifth win of the season.\n\nMartin, despite finishing second, did enough to maintain his championship lead going into the final rounds. The Spanish rider showed remarkable consistency throughout the weekend, demonstrating why he has been the standout performer this season.\n\nThe podium was completed by Enea Bastianini, who put in a strong ride to finish third for Ducati, making it a 1-3 finish for the Italian manufacturer.\n\nWith this result, the championship battle remains wide open heading into the next race, setting up what promises to be an exciting conclusion to one of the most competitive MotoGP seasons in recent memory.",
                NewsCategory.RaceResults,
                sampleAuthorId,
                "bagnaia-wins-valencia-2024"
            ),

            News.Create(
                "Marc Marquez announces switch to factory Ducati for 2024",
                "Eight-time world champion Marc Marquez will join the factory Ducati team next season, ending his long association with Honda.",
                "In a move that has sent shockwaves through the MotoGP paddock, Marc Marquez has confirmed his switch to the factory Ducati team for the 2024 season. The Spanish rider will partner Francesco Bagnaia at the Italian manufacturer, bringing an end to his 11-year association with Honda.\n\nMarquez, who has struggled with injuries and bike performance in recent seasons, sees the move as an opportunity to return to championship-winning form. The 30-year-old rider expressed his excitement about joining the most successful manufacturer in recent MotoGP history.\n\n'This is a new chapter in my career,' Marquez said at the press conference announcing the move. 'Ducati has shown incredible performance in recent years, and I believe this is the best opportunity for me to fight for more championships.'\n\nThe signing represents a major coup for Ducati, who will now have two former world champions leading their factory effort. Team manager Luigi Dall'Igna praised Marquez's signing as 'a dream come true for Ducati.'\n\nThe move also opens up a seat at Honda, with several riders reportedly in contention for the vacant position alongside Joan Mir.",
                NewsCategory.Transfers,
                sampleAuthorId,
                "marquez-ducati-2024"
            ),

            News.Create(
                "Revolutionary aerodynamics package unveiled by Yamaha",
                "Yamaha has introduced a groundbreaking aerodynamics package that could reshape the competitive landscape in MotoGP.",
                "Yamaha has pulled back the curtain on their latest technical innovation – a revolutionary aerodynamics package that the manufacturer believes will significantly improve their competitiveness in the championship fight.\n\nThe new package, developed in collaboration with aerospace specialists, features several cutting-edge elements including redesigned winglets, an innovative fairing design, and a completely new approach to airflow management around the rider.\n\nAccording to Yamaha's technical director, the new aerodynamics package provides up to 15% more downforce while reducing drag by 8% compared to their previous design. These improvements should translate to better cornering stability and increased straight-line speed.\n\n'We've been working on this project for over 18 months,' explained the team's chief engineer. 'The data from our wind tunnel testing and CFD simulations shows remarkable improvements across all areas of performance.'\n\nThe package will make its race debut at the upcoming test session, with both factory riders Fabio Quartararo and Franco Morbidelli eager to experience the improvements firsthand.\n\nThis development comes as Yamaha seeks to close the gap to championship leaders Ducati and return to race-winning form after a challenging period in recent seasons.",
                NewsCategory.Technical,
                sampleAuthorId,
                "yamaha-aerodynamics-2024"
            ),

            News.Create(
                "BREAKING: Unexpected weather forces race postponement",
                "Severe weather conditions at the circuit have forced officials to postpone today's MotoGP race to tomorrow morning.",
                "In an unprecedented turn of events, race officials have been forced to postpone today's MotoGP race due to severe weather conditions that have made the track unsafe for competition.\n\nHeavy rainfall and strong winds have battered the circuit since early morning, creating dangerous conditions that would pose significant risks to rider safety. Track conditions deteriorated rapidly despite efforts by circuit staff to maintain racing standards.\n\nRace Direction made the decision after conducting multiple track inspections and consulting with safety experts and rider representatives. The safety of all participants remains the top priority for MotoGP officials.\n\nThe race has been rescheduled for tomorrow morning at 10:00 AM local time, weather permitting. Ticket holders for today's event will be granted access to tomorrow's rescheduled race at no additional cost.\n\nThis marks only the third time in MotoGP history that a race has been postponed due to weather conditions, highlighting the severity of the current situation.\n\nFans are advised to check official MotoGP channels for the latest updates on tomorrow's race schedule.",
                NewsCategory.Breaking,
                sampleAuthorId,
                "race-postponed-weather"
            ),

            News.Create(
                "Jorge Martin leads championship with two races remaining",
                "The Pramac Ducati rider has extended his championship lead to 14 points with just two races left in the season.",
                "Jorge Martin's championship aspirations received a major boost with his commanding victory at the Malaysian Grand Prix, extending his points lead to 14 with only two races remaining in the season.\n\nThe Spanish rider delivered a flawless performance in challenging conditions at Sepang, starting from pole position and never relinquishing the lead throughout the 20-lap race. His victory marks his seventh win of the season and puts him in prime position to claim his first MotoGP world championship.\n\n'Today was perfect,' Martin said in the post-race interview. 'The bike felt incredible, and I was able to control the race from the front. But the championship isn't over yet – we need to stay focused for the final two races.'\n\nMartin's closest rival, Francesco Bagnaia, could only manage fourth place after struggling with tire degradation in the hot conditions. The defending champion now faces an uphill battle to retain his crown.\n\nWith the championship fight intensifying, both riders will be under immense pressure in the remaining races. Martin needs just 11 points from the final two races to secure the title, while Bagnaia must maximize every opportunity to close the gap.\n\nThe championship battle moves to Qatar next week for the penultimate round of what has been one of the most exciting seasons in recent MotoGP history.",
                NewsCategory.Championship,
                sampleAuthorId,
                "martin-leads-championship"
            ),

            News.Create(
                "Exclusive: Valentino Rossi on the new generation of riders",
                "The MotoGP legend shares his thoughts on today's young stars and the evolution of the sport.",
                "In an exclusive interview, nine-time world champion Valentino Rossi reflected on the current state of MotoGP and shared his insights about the new generation of riders who have taken over the premier class.\n\n'The level of riding today is incredible,' Rossi told us during a recent visit to the Misano circuit. 'These young riders like Martin, Bagnaia, and Quartararo have taken the sport to new heights. Their speed and consistency are remarkable.'\n\nThe Italian legend, who retired from full-time competition in 2021, has been following the championship closely from his role as a team owner in Moto2 and MotoGP.\n\n'What impresses me most is their mental strength,' Rossi continued. 'The pressure in MotoGP today is enormous, but these riders handle it with such maturity. When I was their age, I'm not sure I could have managed the same way.'\n\nRossi also praised the technical evolution of MotoGP, noting how electronics and aerodynamics have transformed the sport while maintaining its fundamental appeal.\n\n'The bikes are more sophisticated now, but the essence of racing remains the same – it's still about who can push the limits furthest while staying in control.'\n\nWhen asked about his proudest achievement, Rossi smiled: 'Helping to grow this sport and inspiring the next generation. Seeing riders like Bagnaia, who trained at my academy, winning championships makes me incredibly proud.'\n\nThe full interview will be available in next month's MotoGP Magazine.",
                NewsCategory.Interviews,
                sampleAuthorId,
                "rossi-interview-new-generation"
            )
        };

        foreach (var news in sampleNews)
        {
            news.Publish();
        }

        // Set some as featured
        sampleNews[0].SetAsFeatured();
        sampleNews[1].SetAsFeatured();
        sampleNews[4].SetAsFeatured();
        
        // Set one as breaking
        sampleNews[3].SetAsBreaking();

        context.Set<News>().AddRange(sampleNews);
        await context.SaveChangesAsync();
    }
}