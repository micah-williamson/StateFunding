using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StateFunding {
  public static class StateFundingHubLabView {
    public static void draw (View Vw, ViewWindow Window) {
      Window.title = "Science Stations";
      InstanceData GameInstance = StateFundingGlobal.fetch.GameInstance;
      Review Rev = GameInstance.ActiveReview;
      Rev.touch ();

      string Description = "Below is a list of existing Science Sations. Having more Science Stations increases State " +
        "Confidence. Landed stations on other Celestial Bodies counts higher than Orbiting Stations. " +
        "To have a qualified Science Station you must have an antenna, a science lab, be able to generate " +
        "power, and have at least one Kerbal on board.";

      ViewLabel DescriptionLabel = new ViewLabel (Description);
      DescriptionLabel.setRelativeTo (Window);
      DescriptionLabel.setLeft (140);
      DescriptionLabel.setTop (20);
      DescriptionLabel.setColor (Color.white);
      DescriptionLabel.setHeight (100);
      DescriptionLabel.setWidth (Window.getWidth () - 140);

      Vw.addComponent (DescriptionLabel);

      ViewLabel TotalCoverage = new ViewLabel ("Orbiting Stations: " + Rev.orbitalScienceStations + ". " +
        "Landed Stations: " + Rev.planetaryScienceStations + ".");
      TotalCoverage.setRelativeTo (Window);
      TotalCoverage.setLeft (140);
      TotalCoverage.setTop (130);
      TotalCoverage.setColor (Color.white);
      TotalCoverage.setHeight (30);
      TotalCoverage.setWidth (Window.getWidth () - 140);

      Vw.addComponent (TotalCoverage);

      ViewScroll StationsScroll = new ViewScroll ();
      StationsScroll.setRelativeTo (Window);
      StationsScroll.setWidth (Window.getWidth () - 140);
      StationsScroll.setHeight (Window.getHeight () - 160);
      StationsScroll.setLeft (140);
      StationsScroll.setTop (150);

      Vw.addComponent(StationsScroll);

      Vessel[] ScienceStations = VesselHelper.GetScienceStations ();

      int labelHeight = 20;

      for (int i = 0; i < ScienceStations.Length; i++) {
        Vessel ScienceStation = ScienceStations [i];
        string action;
        string target;

        if (ScienceStation.Landed) {
          action = "Landed At";
          target = ScienceStation.mainBody.GetName ();
        } else {
          action = "Orbiting";
          target = ScienceStation.GetOrbit ().referenceBody.GetName();
        }

        string label = ScienceStation.GetName () + " is " + action + " " + target;

        ViewLabel StationLabel = new ViewLabel (label);
        StationLabel.setRelativeTo (StationsScroll);
        StationLabel.setTop (labelHeight + (labelHeight + 5) * i);
        StationLabel.setLeft (0);
        StationLabel.setHeight (labelHeight);
        StationLabel.setWidth (StationsScroll.getWidth () - 20);
        StationLabel.setColor (Color.white);

        StationsScroll.Components.Add (StationLabel);
      }
    }
  }
}

