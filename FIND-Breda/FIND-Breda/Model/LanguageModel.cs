﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIND_Breda.Model
{
    public class LanguageModel
    {
        private static LanguageModel _languageController = null;
        private static readonly object _padlock = new object();

        /* Singleton
         * Gebruik: LanguageMode.instance.<methode>() 
         */
        public static LanguageModel instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_languageController == null)
                    {
                        _languageController = new LanguageModel();
                    }
                    return _languageController;
                }
            }
        }

        private Language _language;
        private Dictionary<Text, string> _dutch;
        private Dictionary<Text, string> _english;

        /* Constructor om alle talen te initalizeren */
        public LanguageModel()
        {
            this._language = Language.dutch;
            this._dutch = new Dictionary<Text, string>();
            this._english = new Dictionary<Text, string>();

            /* Hieronder kan je de text van buttons o.i.d. aanpassen, weer eerst enum toevoegen! */
            this._dutch.Add(Text.dutchlanguagebutton, "Nederlands");
            this._dutch.Add(Text.englishlanguagebutton, "Engels");
            this._dutch.Add(Text.plannedroutebutton, "Voorgeplande route kiezen");
            this._dutch.Add(Text.mapbutton, "Kaart");
            this._dutch.Add(Text.sightingbutton, "Bezienswaardigheden");
            this._dutch.Add(Text.helpbutton, "Help & info");
            this._dutch.Add(Text.listofroutelabel, "Lijst van routes");
            this._dutch.Add(Text.getlocationbutton, "Geef locatie async");
            this._dutch.Add(Text.aerialcheckbox, "Lucht weergave");
            this._dutch.Add(Text.aerialwithroadscheckbox, "Lucht+wegen weergave");
            this._dutch.Add(Text.darkthemecheckbox, "Donker thema");
            this._dutch.Add(Text.trafficcheckbox, "Verkeer");
            this._dutch.Add(Text.pedestriancheckbox, "Voetganger");
            this._dutch.Add(Text.sightingslabel, "Bezienswaardigheden");
            this._dutch.Add(Text.monumentscheckbox, "Monumenten");
            this._dutch.Add(Text.buildingscheckbox, "Gebouwen");
            this._dutch.Add(Text.marketcheckbox, "Markten");
            this._dutch.Add(Text.remainingcheckbox, "Overig");
            this._dutch.Add(Text.helptextblock, "Hier komt de beschrijving van de app");
            this._dutch.Add(Text.loadmapfirst, "Je moet de kaart eerst laden!");

            this._english.Add(Text.dutchlanguagebutton, "Dutch");
            this._english.Add(Text.englishlanguagebutton, "English");
            this._english.Add(Text.plannedroutebutton, "Choose planned route");
            this._english.Add(Text.mapbutton, "Map");
            this._english.Add(Text.sightingbutton, "Sightings");
            this._english.Add(Text.helpbutton, "Help & info");
            this._english.Add(Text.listofroutelabel, "List of routes");
            this._english.Add(Text.getlocationbutton, "Give location async");
            this._english.Add(Text.aerialcheckbox, "Aerial view");
            this._english.Add(Text.aerialwithroadscheckbox, "Aerial+road view");
            this._english.Add(Text.darkthemecheckbox, "Dark theme");
            this._english.Add(Text.trafficcheckbox, "Traffic");
            this._english.Add(Text.pedestriancheckbox, "Walker");
            this._english.Add(Text.sightingslabel, "Sightings");
            this._english.Add(Text.monumentscheckbox, "Monuments");
            this._english.Add(Text.buildingscheckbox, "Buildings");
            this._english.Add(Text.marketcheckbox, "Marketplaces");
            this._english.Add(Text.remainingcheckbox, "Other sightings");
            this._english.Add(Text.helptextblock, "Here will be the description of the app");
            this._english.Add(Text.loadmapfirst, "You have to load the map first!");
        }

        /* Methode om de taal te veranderen */
        public void setLanguage(Language language)
        {
            this._language = language;
        }


        /* Methode om een string van tekst te geven met de default taal */
        public String getText(Text text)
        {
            return this.getText(text, this._language);
        }

        /* Methode om een string van tekst te geven met de geselecteerde taal */
        public String getText(Text text, Language lang)
        {
            String result = "";

            switch (lang)
            {
                case Language.english:
                    result = _english[text];
                    break;
                case Language.dutch:
                default:
                    result = _dutch[text];
                    break;
            }
            return result;
        }

    }
}