// Copyright (C) 2013 Paul G Brown

using System.Collections.Generic;

namespace StatesAndProvinces
{
	/// <summary>
	/// Creates the list of subregions for the individual countries.
	/// </summary>
	public static class Factory
	{
		/// <summary>
		/// Public member for creating a list of subregions.
		/// </summary>
		/// <param name="selection">The country or countries that define the desired subregions.</param>
		/// <returns>A generic List of Subregions.</returns>
		/// <remarks>
		/// More that one country may be returned by using the bitwise OR operator.
		/// </remarks>
		public static List<SubRegion> Make(CountrySelection selection)
		{
			var results = new List<SubRegion>();

			if (selection.HasFlag(CountrySelection.Canada))
			{
				results.AddRange(MakeCanadianProvinces());
			}

			if (selection.HasFlag(CountrySelection.UnitedStates))
			{
				results.AddRange(MakeUSStates());
			}

			if (results.Count == 0)
			{
				throw new System.NotImplementedException("The country selection has not been implemented.");
			}
			return results;
		}

		/// <summary>
		/// Creates the list of Canadian provinces.
		/// </summary>
		/// <returns>A generic List of provinces.</returns>
		private static List<SubRegion> MakeCanadianProvinces()
		{
            var results = new List<SubRegion>
            {
                new SubRegion() { Abbreviation = "AB", Name = "Alberta", AlternateAbbreviation = "Alta.", IsoCode = "CA-AB" },
                new SubRegion() { Abbreviation = "BC", Name = "British Columbia", AlternateAbbreviation = "B.C.", IsoCode = "CA-BC" },
                new SubRegion() { Abbreviation = "MB", Name = "Manitoba", AlternateAbbreviation = "Man.", IsoCode = "CA-MB" },
                new SubRegion() { Abbreviation = "NB", Name = "New Brunswick", AlternateAbbreviation = "N.B.", IsoCode = "CA-NB" },
                new SubRegion() { Abbreviation = "NL", Name = "Newfoundland and Labrador", AlternateAbbreviation = "Nfld.", IsoCode = "CA-NL" },
                new SubRegion() { Abbreviation = "NS", Name = "Nova Scotia", AlternateAbbreviation = "N.S.", IsoCode = "CA-NS" },
                new SubRegion() { Abbreviation = "NT", Name = "Northwest Territories", AlternateAbbreviation = "N.W.T.", IsoCode = "CA-NT" },
                new SubRegion() { Abbreviation = "NU", Name = "Nunavut", AlternateAbbreviation = "Nun.", IsoCode = "CA-NU" },
                new SubRegion() { Abbreviation = "ON", Name = "Ontario", AlternateAbbreviation = "Ont.", IsoCode = "CA-ON" },
                new SubRegion() { Abbreviation = "PE", Name = "Prince Edward Island", AlternateAbbreviation = "P.E.I.", IsoCode = "CA-PE" },
                new SubRegion() { Abbreviation = "QC", Name = "Quebec", AlternateAbbreviation = "Que.", IsoCode = "CA-QC" },
                new SubRegion() { Abbreviation = "SK", Name = "Saskatchewan", AlternateAbbreviation = "Sask.", IsoCode = "CA-SK" },
                new SubRegion() { Abbreviation = "YT", Name = "Yukon", AlternateAbbreviation = "Yuk.", IsoCode = "CA-YT" }
            };
            return results;
		}

		/// <summary>
		/// Creates the list of US states.
		/// </summary>
		/// <returns>A generic List of US states.</returns>
		private static List<SubRegion> MakeUSStates()
		{
            var results = new List<SubRegion>
            {
                new SubRegion() { Name = "Alabama", IsoCode = "US-AL", Abbreviation = "AL", AlternateAbbreviation = "Ala." },
                new SubRegion() { Name = "Alaska", IsoCode = "US-AK", Abbreviation = "AK", AlternateAbbreviation = "Alaska" },
                new SubRegion() { Name = "Arizona", IsoCode = "US-AZ", Abbreviation = "AZ", AlternateAbbreviation = "Ariz." },
                new SubRegion() { Name = "Arkansas", IsoCode = "US-AR", Abbreviation = "AR", AlternateAbbreviation = "Ark." },
                new SubRegion() { Name = "California", IsoCode = "US-CA", Abbreviation = "CA", AlternateAbbreviation = "Calif." },
                new SubRegion() { Name = "Colorado", IsoCode = "US-CO", Abbreviation = "CO", AlternateAbbreviation = "Colo." },
                new SubRegion() { Name = "Connecticut", IsoCode = "US-CT", Abbreviation = "CT", AlternateAbbreviation = "Conn." },
                new SubRegion() { Name = "Delaware", IsoCode = "US-DE", Abbreviation = "DE", AlternateAbbreviation = "Del." },
                new SubRegion() { Name = "District of Columbia", IsoCode = "US-DC", Abbreviation = "DC", AlternateAbbreviation = "D.C." },
                new SubRegion() { Name = "Florida", IsoCode = "US-FL", Abbreviation = "FL", AlternateAbbreviation = "Fla." },
                new SubRegion() { Name = "Georgia", IsoCode = "US-GA", Abbreviation = "GA", AlternateAbbreviation = "Ga." },
                new SubRegion() { Name = "Hawaii", IsoCode = "US-HI", Abbreviation = "HI", AlternateAbbreviation = "Hawaii" },
                new SubRegion() { Name = "Idaho", IsoCode = "US-ID", Abbreviation = "ID", AlternateAbbreviation = "Idaho" },
                new SubRegion() { Name = "Illinois", IsoCode = "US-IL", Abbreviation = "IL", AlternateAbbreviation = "Ill." },
                new SubRegion() { Name = "Indiana", IsoCode = "US-IN", Abbreviation = "IN", AlternateAbbreviation = "Ind." },
                new SubRegion() { Name = "Iowa", IsoCode = "US-IA", Abbreviation = "IA", AlternateAbbreviation = "Iowa" },
                new SubRegion() { Name = "Kansas", IsoCode = "US-KS", Abbreviation = "KS", AlternateAbbreviation = "Kan." },
                new SubRegion() { Name = "Kentucky", IsoCode = "US-KY", Abbreviation = "KY", AlternateAbbreviation = "Ky." },
                new SubRegion() { Name = "Louisiana", IsoCode = "US-LA", Abbreviation = "LA", AlternateAbbreviation = "La." },
                new SubRegion() { Name = "Maine", IsoCode = "US-ME", Abbreviation = "ME", AlternateAbbreviation = "Maine" },
                new SubRegion() { Name = "Maryland", IsoCode = "US-MD", Abbreviation = "MD", AlternateAbbreviation = "Md." },
                new SubRegion() { Name = "Massachusetts", IsoCode = "US-MA", Abbreviation = "MA", AlternateAbbreviation = "Mass." },
                new SubRegion() { Name = "Michigan", IsoCode = "US-MI", Abbreviation = "MI", AlternateAbbreviation = "Mich." },
                new SubRegion() { Name = "Minnesota", IsoCode = "US-MN", Abbreviation = "MN", AlternateAbbreviation = "Minn." },
                new SubRegion() { Name = "Mississippi", IsoCode = "US-MS", Abbreviation = "MS", AlternateAbbreviation = "Miss." },
                new SubRegion() { Name = "Missouri", IsoCode = "US-MO", Abbreviation = "MO", AlternateAbbreviation = "Mo." },
                new SubRegion() { Name = "Montana", IsoCode = "US-MT", Abbreviation = "MT", AlternateAbbreviation = "Mont." },
                new SubRegion() { Name = "Nebraska", IsoCode = "US-NE", Abbreviation = "NE", AlternateAbbreviation = "Neb." },
                new SubRegion() { Name = "Nevada", IsoCode = "US-NV", Abbreviation = "NV", AlternateAbbreviation = "Nev." },
                new SubRegion() { Name = "New Hampshire", IsoCode = "US-NH", Abbreviation = "NH", AlternateAbbreviation = "N.H." },
                new SubRegion() { Name = "New Jersey", IsoCode = "US-NJ", Abbreviation = "NJ", AlternateAbbreviation = "N.J." },
                new SubRegion() { Name = "New Mexico", IsoCode = "US-NM", Abbreviation = "NM", AlternateAbbreviation = "N.M." },
                new SubRegion() { Name = "New York", IsoCode = "US-NY", Abbreviation = "NY", AlternateAbbreviation = "N.Y." },
                new SubRegion() { Name = "North Carolina", IsoCode = "US-NC", Abbreviation = "NC", AlternateAbbreviation = "N.C" },
                new SubRegion() { Name = "North Dakota", IsoCode = "US-ND", Abbreviation = "ND", AlternateAbbreviation = "N.D." },
                new SubRegion() { Name = "Ohio", IsoCode = "US-OH", Abbreviation = "OH", AlternateAbbreviation = "Ohio" },
                new SubRegion() { Name = "Oklahoma", IsoCode = "US-OK", Abbreviation = "OK", AlternateAbbreviation = "Okla." },
                new SubRegion() { Name = "Oregon", IsoCode = "US-OR", Abbreviation = "OR", AlternateAbbreviation = "Ore." },
                new SubRegion() { Name = "Pennsylvania", IsoCode = "US-PA", Abbreviation = "PA", AlternateAbbreviation = "Pa." },
                new SubRegion() { Name = "Rhode Island", IsoCode = "US-RI", Abbreviation = "RI", AlternateAbbreviation = "R.I." },
                new SubRegion() { Name = "South Carolina", IsoCode = "US-SC", Abbreviation = "SC", AlternateAbbreviation = "S.C." },
                new SubRegion() { Name = "South Dakota", IsoCode = "US-SD", Abbreviation = "SD", AlternateAbbreviation = "S.D." },
                new SubRegion() { Name = "Tennessee", IsoCode = "US-TN", Abbreviation = "TN", AlternateAbbreviation = "Tenn." },
                new SubRegion() { Name = "Texas", IsoCode = "US-TX", Abbreviation = "TX", AlternateAbbreviation = "Texas" },
                new SubRegion() { Name = "Utah", IsoCode = "US-UT", Abbreviation = "UT", AlternateAbbreviation = "Utah" },
                new SubRegion() { Name = "Vermont", IsoCode = "US-VT", Abbreviation = "VT", AlternateAbbreviation = "Vt." },
                new SubRegion() { Name = "Virginia", IsoCode = "US-VA", Abbreviation = "VA", AlternateAbbreviation = "Va." },
                new SubRegion() { Name = "Washington", IsoCode = "US-WA", Abbreviation = "WA", AlternateAbbreviation = "Wash." },
                new SubRegion() { Name = "West Virginia", IsoCode = "US-WV", Abbreviation = "WV", AlternateAbbreviation = "W.Va." },
                new SubRegion() { Name = "Wisconsin", IsoCode = "US-WI", Abbreviation = "WI", AlternateAbbreviation = "Wis." },
                new SubRegion() { Name = "Wyoming", IsoCode = "US-WY", Abbreviation = "WY", AlternateAbbreviation = "Wyo." }
            };
            return results;
		}
	}
}
