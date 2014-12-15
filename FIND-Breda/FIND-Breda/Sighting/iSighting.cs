using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace FIND_Breda.Sighting
{
    interface iSighting
    {
        private String _name;
        private String _shortInfo;
        private String _adress;
        private String _fullInfo;
        private double _distanceFromUser;
        private double _mapLocation;
        private Image _image;

        public Image iSighting(String _name, String _shortInfo, String _adress, String _fullInfo, double _distanceFromUser, double _mapLocation, Image _image);
    }
}
