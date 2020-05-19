using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewsManager : MonoBehaviour
{
    public TMP_Text newsArticle_Text;

    public Button breakingNews;
    public Button tipOfTheDay;
    public Button moreNews;
    public Button forecast;

    void Start()
    {
        breakingNews.onClick.AddListener(LoadBreakingNews);
        tipOfTheDay.onClick.AddListener(LoadTipOfTheDay);
        moreNews.onClick.AddListener(LoadMoreNews);
        forecast.onClick.AddListener(LoadForecast);
    }

    private void LoadBreakingNews()
    {
        string text = "This is the breaking news section";

        switch (PlayerPrefs.GetInt("CurrentWeek"))
        {
            case 0:
                text = breakingNews_1;
                break;
            case 1:
                text = breakingNews_2;
                break;
            case 2:
                text = breakingNews_3;
                break;
            case 3:
                text = breakingNews_4;
                break;
            case 4:
                text = breakingNews_5;
                break;
            case 5:
                text = breakingNews_6;
                break;
            case 6:
                text = breakingNews_7;
                break;

            default:
                break;
        }

        newsArticle_Text.text = text;
    }

    private void LoadTipOfTheDay()
    {
        newsArticle_Text.text = "This is the tip of the day section";
    }

    private void LoadMoreNews()
    {
        string text = "This is the more news section";

        switch (PlayerPrefs.GetInt("CurrentWeek"))
        {
            case 0:
                text = moreNews_1;
                break;
            case 1:
                text = moreNews_2;
                break;
            case 2:
                text = moreNews_3;
                break;
            case 3:
                text = moreNews_4;
                break;
            case 4:
                text = moreNews_5;
                break;
            case 5:
                text = moreNews_6;
                break;
            case 6:
                text = moreNews_7;
                break;

            default:
                break;
        }

        newsArticle_Text.text = text;
    }

    private void LoadForecast()
    {
        string text = "This is the forecast section";

        switch (PlayerPrefs.GetInt("CurrentWeek"))
        {
            case 0:
                text = forecast_1;
                break;
            case 1:
                text = forecast_2;
                break;
            case 2:
                text = forecast_3;
                break;
            case 3:
                text = forecast_4;
                break;
            case 4:
                text = forecast_5;
                break;
            case 5:
                text = forecast_6;
                break;
            case 6:
                text = forecast_7;
                break;

            default:
                break;
        }

        newsArticle_Text.text = text;
    }


    private string breakingNews_1 = "Virus Fears weaken Wall Street\n\nFears over COVID-19’s effect on the world economy have put a damper on the US stock market." +
        "The Dow-Jones, Nasdaq and S&P all sank following news of the rapid spread of the disease.Airlines have started cancelling flights to China, " +
        "and President Trump has put out a temporary ban on flights for infected persons. If the 2002/2003 Sars-Pandemic is anything to go by, the Asian stock market " +
        "will mostly suffer, and we will see a stabilization as soon as the number of infected stops rising.";
    private string breakingNews_2 = "Hopes for economic recovery boost US Stock Market\n\nFears surrounding the Corona outbreak have been weakened thanks to a recent" +
        " upward trend in the US stock Market.The Dow-Jones, S&P and Nasdaq have all risen thanks to major companies such as Microsoft (+2.6%), Amazon(+2.6%) and Alphabet(+2%)." +
        " After the holidays, China is now also returning to normal activity, which shows a promising future. “Some people think that the Corona outbreak will mostly affect China”" +
        " says market expert Peter Kenny, “But the numbers reported by firms are very strong, and this is reflected in the current trend”. On the same note, the US Dollar has reached" +
        " its highest value in 4 months.";
    private string breakingNews_3 = "DAX remains unchanged after a turbulent week\n\nAfter suffering a heavy loss as well as three record highs all within the same week, " +
        "the German Dax ends the week almost unchanged.Fears surrounding the COVID-19 outbreak have been mostly cast aside, with heavy infected numbers being attributed " +
        "to new testing methods.As long as infection rates keep sinking, this will likely remain this way.Meanwhile, the Euro has reached its lowest point in three years.";
    private string breakingNews_4 = "Dax ends week on a negative note – Corona virus is troubling investors\n\nNow that the COVID-19 virus is spreading to South Korea " +
        "and Japan, investors are starting to worry.This Friday, the South Korean government announced 52 new cases, bringing the total number up to 150, causing " +
        "investors to be doubtful of a quick end to the pandemic.The Chinese government expects heavy losses in the import and export sector, said Li Xingqian, " +
        "director of foreign trade at the ministry.Additionally, the Chinese automobile industry has sunk by 92% in the first 2 weeks of February, with only 811 " +
        "vehicles sold daily.Fears over the pandemic have caused investors to rely on safe investments, such as gold, which has reached its highest value in 7 years," +
        " at 1631.01$ per ounce.";
    private string breakingNews_5 = "US Stock Market suffers from worst week since financial crisis\n\nPanic surrounding COVID-19 spreads at the stock market, " +
        "with losses in the billions of dollars, with no end in sight. “Investors are preparing for the worst” says stock market expert John Lau, “These are" +
        " uncertain times, no one has answers, and the markets are panicking”. Things have not been this bad since the crash in October of 2008. Still, Wall Stree" +
        "t was able to recoup some losses thanks to a show of Federal support: “We will make use of our tools and act accordingly to support the US economy”, sai" +
        "d Jerome Powell, head of the Federal Reserve.This statement helped alleviate some losses by calming the market.";
    private string breakingNews_6 = "Dax closes more than 3% down – Banks under pressure\n\nAfter it seemed like the Dax was recovering, it was" +
        " sinking again by Thursday, even achieving a record low(7 months) on Friday, with -4.1%. Meanwhile, the US stock market has not been performin" +
        "g much better, with a rapid decrease at the start of the week(-2.9%), and only small gains since.Surprisingly good news from the US job market " +
        "could barely stabilize the stock market, since the numbers originated from before the COVID-19 outbreak.The price of gold is still rising, havin" +
        "g now reached 1690$, and investors are moving more and more money to more secure investments, while returns on “less secure” other investments ma" +
        "y sink as low as 0%, according to Shaun Roach.";
    private string breakingNews_7 = "US Stock Market sees huge increase\n\nFollowing an announcement by President Trump declaring a national emergency state, " +
        "and promising financial aids of up to 50 billion dollars, the US stock market saw its biggest daily increase since 2008. The promise to spend money on" +
        " the containment of the virus seems to have calmed investors down, which was reflected in the Dow Jones(+9.4%), Nasdaq(+9.3%) and S&P(+9.3%). After" +
        " Trump’s surprising announcement on Thursday to ban all European visitors, the Dow Jones had lost 10 percent, the biggest loss since the famed “Black " +
        "Monday” in 1987, but thanks to the promised financial aids to hospitals and communes, these losses could quickly be recouped.";


    private string moreNews_1 = "Nearly four years after the UK voted to leave the European Union, Brexit has finally happened. As the clock struck 11.00.p.m.GMT," +
        " the Article 50 process by which a member state leaves the EU expired and the UK has now entered the transition process it agreed with the bloc.For the first" +
        " time ever, the EU is down a member state. It's a monumental moment that will go down in history, for better or worse.";
    private string moreNews_2 = "A fox sneaked into the British Parliament and caused mayhem. The UK's Parliament witnessed a scene of even more cunning than usual after a" +
        " fox rode an escalator and sneaked into the building. The wily creature evaded capture by police and caused havoc as it padded along the corridors over " +
        "four stories of Parliament's Portcullis House building on Thursday night. The fox had no qualms about showing lawmakers what it thought of them.";
    private string moreNews_3 = "The Tokyo 2020 Olympics remain 'on track' despite the novel coronavirus outbreak in Asia, a top official said Friday. The virus, " +
        "which originated in the Chinese city of Wuhan, has infected more than 64,000 people worldwide and claimed 1,383 lives. The current advice from the World" +
        " Health Organization (WHO) states that 'there's no case for any contingency plans of canceling the Games or moving the Games,' according to International " +
        "Olympic Committee (IOC) member John Coates.";
    private string moreNews_4 = "Protesters attacked a convoy of buses carrying Ukrainian citizens and other nationals evacuated from the Chinese city of Wuhan, " +
        "injuring nine police officers and one civilian Thursday. As global fears and misconceptions spread about the coronavirus outbreak that originated in the city, " +
        "protesters blocked roads in the Ukrainian town of Noviy Sanzhari, where 72 evacuees are to be monitored for two weeks at a medical center. Ukraine has no " +
        "diagnosed cases of the novel coronavirus. The Ukrainian Ministry of Internal Affairs said that 'aggressive citizens' began to pelt the buses with stones, " +
        "and that one man tried to hit police with a car.";
    private string moreNews_5 = "Olympic organizers have insisted that preparations for Tokyo 2020 are going ahead 'as planned' despite the novel coronavirus" +
        " outbreak in Asia. The virus has so far claimed the lives of over 2,700 worldwide, with the majority recorded in mainland China.A number of sporting " +
        "events have already been impacted, including the Tokyo marathon which has been restricted to elite athletes only. '(We) will continue to collaborate " +
        "with all relevant organizations which carefully monitor any incidence of infectious diseases and will review any countermeasures that may be necessary " +
        "with all relevant organizations,' the local Olympic organizing committee said in a statement to CNN Tuesday.";
    private string moreNews_6 = "The Premier League, the biggest soccer league in the world, will no longer shake hands before matches amidst rising coronavirus" +
        " concerns. The league made the announcement Thursday, saying that the fair-play handshake will be gone 'until further notice based on medical advice' a " +
        "statement from the league reads.But the rest of the pre-game ritual will be the same, the league said. 'Clubs and match officials will still perform the " +
        "rest of the traditional walk-out protocol ahead of each fixture,' the statement says. 'On entering the field of play, the two teams will continue to line up," +
        " accompanied by the Premier League music, then players from the home team will walk past their opposition without shaking their hands.'";
    private string moreNews_7 = "Pope Francis defied Italy's lockdown on Sunday afternoon, leaving his home in the Vatican to pray for those affected by" +
        " the novel coronavirus at a famous crucifix that believers claim helped to save Romans from the plague in 1522. The Pope stopped his Ford Focus " +
        "car near the Church of San Marcello in Rome's city center, where the crucifix is kept, in order to walk to the church as a sign of pilgrimage, the" +
        " Vatican said. 'The Holy Father pleaded for an end to the pandemic that has struck Italy and the world,' Vatican spokesman Matteo Bruni said in a statement. " +
        "The Pope also called for 'the healing of the many sick, remembered the numerous victims of these past days and asked that their families and friends might find" +
        " consolation and comfort.' He prayed for doctors, nurses and other healthcare workers, the Vatican said.";


    private string forecast_1 = "The weather today will be mostly cloudy with some light rain, and temperatures ranging from 7 to 13 degrees. Expect a clear night.";
    private string forecast_2 = "Expect a lot of sun today, partly covered by light clouds. Don't take the summer clothes out of storage though, as temperatures will " +
        "remain quite low, ranging from 5 to 7 degrees celsius. If you're out late at night, bring an umbrella, as we are expecting some light rain.";
    private string forecast_3 = "The weather should be mostly sunny today, but we still recommend staying indoors, as temperatures will not exceed 6 degrees. The night should" +
        "be foggy with temperatures dropping as low as 1 degree, so watch out for ice on the roads tomorrow!";
    private string forecast_4 = "Expect to see some sun with a lot of clouds today. Temperatures will remain accordingly low, ranging from 4 to 8 degrees. The sky" +
        " should clear up in the evening.";
    private string forecast_5 = "Bring an umbrella if plan on going out this morning, as we are expecting rain. While the rain should go away in the afternoon, it'll remain" +
        " cloudy for the rest of the day, as well as the night. Temperatures should stay low, between 1 and 5 degrees.";
    private string forecast_6 = "Temperatures should remain low today, never exceeding 10 degrees, but at least it will be mostly sunny. The night will be clear at first, " +
        "followed by rain in the morning and temperatures as low as 1 degree.";
    private string forecast_7 = "Has spring finally arrived? We expect a rise in temperature today up to 17 degrees, under a sunny sky with some passing clouds. The night " +
        "should remain clear as well, but if you plan on going out with your friends remember to keep contact to a minimum.";
}
