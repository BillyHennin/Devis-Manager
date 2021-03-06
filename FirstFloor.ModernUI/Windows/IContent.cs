﻿// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from FirstFloor.ModernUI INC. team.
//  
// Copyrights (c) 2014 FirstFloor.ModernUI INC. All rights reserved.

using FirstFloor.ModernUI.Windows.Navigation;

namespace FirstFloor.ModernUI.Windows
{
    public interface IContent
    {
        void OnFragmentNavigation(FragmentNavigationEventArgs e);

        void OnNavigatedFrom(NavigationEventArgs e);

        void OnNavigatedTo(NavigationEventArgs e);

        void OnNavigatingFrom(NavigatingCancelEventArgs e);
    }
}