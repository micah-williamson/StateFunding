using System;
using UnityEngine;
using System.Collections;

namespace StateFunding {
  public class StateFundingHubView: View {
    private ViewWindow Window;
    private ArrayList SideMenu;

    public StateFundingHubView () {
      createWindow ();
    }

    private void createWindow() {
      reloadBase ();
    }

    private void reloadBase() {
      this.removeAll ();

      SideMenu = new ArrayList ();

      Window = new ViewWindow ("");
      Window.setWidth (800);
      Window.setHeight (Screen.height - 200);
      Window.setLeft ((Screen.width-800)/2);
      Window.setTop (100);

      this.addComponent (Window);

      SideMenu.Add(new ViewButton("Current State", LoadCurrentState));
      SideMenu.Add(new ViewButton("Space Stations", LoadSpaceStations));
      SideMenu.Add(new ViewButton("Bases", LoadBases));
      SideMenu.Add(new ViewButton("Sat Coverage", LoadSatelliteCoverage));
      SideMenu.Add(new ViewButton("Science Stations", LoadScienceStations));
      SideMenu.Add(new ViewButton("Mining Rigs", LoadMiningRigs));
      SideMenu.Add(new ViewButton("Rovers", LoadRovers));
      SideMenu.Add(new ViewButton("Kerbals", LoadKerbals));
      SideMenu.Add(new ViewButton("Past Reviews", LoadPastReviews));

      for (var i = 0; i < SideMenu.ToArray ().Length; i++) {
        ViewButton Btn = (ViewButton)SideMenu.ToArray () [i];
        Btn.setRelativeTo (Window);
        Btn.setLeft (10);
        Btn.setTop (10 + i * 45);
        Btn.setWidth (120);
        Btn.setHeight (35);
        this.addComponent (Btn);
      }
    }

    private void LoadSatelliteCoverage() {
      reloadBase ();
      StateFundingHubCoverageView.draw (this, Window);
    }

    private void LoadBases() {
      reloadBase ();
      StateFundingHubBasesView.draw (this, Window);
    }

    private void LoadSpaceStations() {
      reloadBase ();
      StateFundingHubStationsView.draw (this, Window);
    }

    private void LoadRovers() {
      reloadBase ();
      StateFundingHubRoversView.draw (this, Window);
    }

    private void LoadScienceStations() {
      reloadBase ();
      StateFundingHubLabView.draw (this, Window);
    }

    private void LoadMiningRigs() {
      reloadBase ();
      StateFundingHubMiningView.draw (this, Window);
    }

    private void LoadKerbals() {
      reloadBase ();
      StateFundingHubKerbalsView.draw (this, Window);
    }

    private void LoadCurrentState() {
      reloadBase ();
      StateFundingHubCurrentView.draw (this, Window);
    }

    private void LoadPastReviews() {
      reloadBase ();
      StateFundingHubReviewsView.draw (this, Window);
    }
  }
}

