using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StateFunding {
  public static class StateFundingHubStationsView {
    public static void draw (View Vw, ViewWindow Window) {
      Window.title = "Space Stations";
      Instance GameInstance = StateFundingGlobal.fetch.GameInstance;
      Review Rev = GameInstance.ActiveReview;
      Rev.touch ();

      string Description = "Below is a list of existing Space Stations. Vessels that are Space Stations should be labeled as " +
        "such. Space Stations increase State Confidence as well as Public Opinion. Space Stations are scored by the following " +
        "criteria: Total Fuel, Total Ore, Crew, Crew Capacity, Docking Port Count, Docked Vessels (aka station modules) and if it " +
        "has a science lab. If the Station is landed on an astroid it will also get a bonus- higher bonus if you have a drill. Stations " +
        "must be able to generate their own power.";

      ViewLabel DescriptionLabel = new ViewLabel (Description);
      DescriptionLabel.setRelativeTo (Window);
      DescriptionLabel.setLeft (140);
      DescriptionLabel.setTop (20);
      DescriptionLabel.setColor (Color.white);
      DescriptionLabel.setHeight (100);
      DescriptionLabel.setWidth (Window.getWidth () - 140);

      Vw.addComponent (DescriptionLabel);

      ViewLabel TotalStations = new ViewLabel ("Total Stations: " + Rev.SpaceStations.Length);
      TotalStations.setRelativeTo (Window);
      TotalStations.setLeft (140);
      TotalStations.setTop (130);
      TotalStations.setColor (Color.white);
      TotalStations.setHeight (30);
      TotalStations.setWidth (Window.getWidth () - 140);

      Vw.addComponent (TotalStations);

      ViewScroll StationsScroll = new ViewScroll ();
      StationsScroll.setRelativeTo (Window);
      StationsScroll.setWidth (Window.getWidth () - 140);
      StationsScroll.setHeight (Window.getHeight () - 160);
      StationsScroll.setLeft (140);
      StationsScroll.setTop (150);

      Vw.addComponent(StationsScroll);

      Vessel[] Stations = VesselHelper.GetSpaceStations ();

      int labelHeight = 20;

      for (int i = 0; i < Stations.Length; i++) {
        Vessel Station = Stations [i];
        string target;

        string label = Station.GetName () + " is Orbiting " + Station.GetOrbit().referenceBody.GetName ();

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

