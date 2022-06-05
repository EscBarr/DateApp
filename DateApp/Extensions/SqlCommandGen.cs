namespace DateApp.Extensions
{
    public static class SqlCommandGen
    {
        //SELECT* FROM public."Users" WHERE "Birthday" BETWEEN(current_date - '25 years'::interval) AND(current_date - '18 years'::interval) AND "Adress" = 'Астрахань' и так со всеми долбанными кавычками
        public static string GenBaseRequest(bool Gender)
        {
            return "SELECT * FROM \"public\".\"Users\" WHERE \"Gender\" = " + Gender.ToString() + " " + "AND \"Is_Participating\" = True" + " ";
        }

        public static void Add_DateCheck(ref string Query, string AgeTo, string AgeFrom)
        {
            Query += "AND \"Birthday\" BETWEEN(current_date - '" + AgeFrom + "  years'::interval) AND (current_date - '" + AgeTo + " years'::interval)" + " ";
        }

        public static void Add_AdressCheck(ref string Query, string arg)
        {
            Query += "AND \"Adress\" = '" + arg + "'" + " ";
        }

        public static void Add_ReligionCheck(ref string Query, string arg)
        {
            Query += "AND \"ReligionType\" = '" + arg + "'" + " ";
        }

        public static void Add_EducationCheck(ref string Query, string arg)
        {
            Query += "AND \"Education\" = '" + arg + "'" + " ";
        }

        public static void Add_HeightCheck(ref string Query, string arg)
        {
            Query += "AND \"Height\" > '" + arg + "'" + " ";
        }

        public static void Add_WeightCheck(ref string Query, string arg)
        {
            Query += "AND \"Weight\" < '" + arg + "'" + " ";
        }

        public static void Add_EyeColorCheck(ref string Query, string arg)
        {
            Query += "AND \"EyeColor\" = '" + arg + "'" + " ";
        }

        public static void Add_OpinionOnSmokingCheck(ref string Query, string arg)
        {
            Query += "AND \"OpinionOnSmoking\" = '" + arg + "'" + " ";
        }

        public static void Add_OpinionOnAlcoholCheck(ref string Query, string arg)
        {
            Query += "AND \"OpinionOnAlcohol\" = '" + arg + "'" + " ";
        }
    }
}