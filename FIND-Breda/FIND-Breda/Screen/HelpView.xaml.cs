﻿using FIND_Breda.Common;
using FIND_Breda.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FIND_Breda.Screen
{
    public sealed partial class HelpView : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public HelpView()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HelpTextBlock.Text = LanguageModel.instance.getText(Text.helptextblock);
            ZoomInAndOutButton.Content = LanguageModel.instance.getText(Text.ZoomInAndOutButton);
            BackButtonButton.Content = LanguageModel.instance.getText(Text.BackButtonButton);
            MapInfoButton.Content = LanguageModel.instance.getText(Text.MapInfoButton);
            LegendButton.Content = LanguageModel.instance.getText(Text.LegendButton);
            SightingInfoButton.Content = LanguageModel.instance.getText(Text.SightingInfoButton);
            this.navigationHelper.OnNavigatedTo(e);
        }

        private void ZoomInAndOutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ZoomInAndOutInfo));
        }

        private void MapInfoButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MapInfo));
        }

        private void BackButtonInfoButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BackButtonInfo));
        }

        private void LegendButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LegendInfo));
        }

        private void SightingInfoButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SightingInfo));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Contact_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FeedbackView));
        }

    }
}
