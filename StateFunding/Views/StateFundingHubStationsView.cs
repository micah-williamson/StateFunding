using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StateFunding {
  public static class StateFundingHubStationsView {
    public static void draw (View Vw, ViewWindow Window) {
      Window.title = "Space Stations";
      InstanceData GameInstance = StateFundingGlobal.fetch.GameInstance;
      Review Rev = GameInstance.ActiveReview;
      Rev.touch ();

      string Description = "Below is a list of existing Space Stations. Vessels that are Space Stations should be labeled as " +
        "such, be in orbit, and must be able to generate their own power. Space Stations increase State Confidence as well as Public Opinion." +
        "Space Stations are scored by the following criteria: Total Fuel (SC), Total Ore (SC), Crew (PO), Crew Capacity (SC), Docking " +
        "Port Count (SC), Docked Vessels (PO) and if it has a science lab (SC/PO). If the Station is landed on an astroid it will also " +
        "get a bonus (PO). If you are on an astroid you will also get a bonus for having a drill (SC/PO).";

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

      SpaceStationReport[] Stations = Rev.SpaceStations;

      for (int i = 0; i < Stations.Length; i++) {
        drawItem (Stations [i], StationsScroll, i);
      }

    }

    public static void drawItem(SpaceStationReport Station, ViewScroll parent, int offset) {
      int boxHeight = 110;

      ViewBox Box = new ViewBox ();
      Box.setRelativeTo (parent);
      Box.setWidth (parent.getWidth () - 20);
      Box.setHeight (boxHeight);
      Box.setLeft (0);
      Box.setTop ((boxHeight + 10) * offset);
      Box.setColor (Color.white);
      parent.Components.Add (Box);

      string label = "[" + Station.name + " is Orbiting " + Station.entity + "]";
      ViewLabel StationLabel = new ViewLabel (label);
      StationLabel.setRelativeTo (Box);
      StationLabel.setTop (5);
      StationLabel.setLeft (5);
      StationLabel.setHeight (15);
      StationLabel.setPercentWidth (100);
      StationLabel.setColor (Color.green);
      parent.Components.Add (StationLabel);

      ViewLabel FuelLabel = new ViewLabel ("Fuel: " + Station.fuel);
      FuelLabel.setRelativeTo (Box);
      FuelLabel.setTop (25);
      FuelLabel.setLeft (5);
      FuelLabel.setHeight (15);
      FuelLabel.setWidth (150);
      FuelLabel.setColor (Color.white);
      parent.Components.Add (FuelLabel);

      ViewLabel OreLabel = new ViewLabel ("Ore: " + Station.ore);
      OreLabel.setRelativeTo (Box);
      OreLabel.setTop (45);
      OreLabel.setLeft (5);
      OreLabel.setHeight (20);
      OreLabel.setWidth (150);
      OreLabel.setColor (Color.white);
      parent.Components.Add (OreLabel);

      ViewLabel CrewLabel = new ViewLabel ("Crew: " + Station.crew);
      CrewLabel.setRelativeTo (Box);
      CrewLabel.setTop (65);
      CrewLabel.setLeft (5);
      CrewLabel.setHeight (20);
      CrewLabel.setWidth (150);
      CrewLabel.setColor (Color.white);
      parent.Components.Add (CrewLabel);

      ViewLabel CrewCapacityLabel = new ViewLabel ("Crew Capacity: " + Station.crewCapacity);
      CrewCapacityLabel.setRelativeTo (Box);
      CrewCapacityLabel.setTop (85);
      CrewCapacityLabel.setLeft (5);
      CrewCapacityLabel.setHeight (20);
      CrewCapacityLabel.setWidth (150);
      CrewCapacityLabel.setColor (Color.white);
      parent.Components.Add (CrewCapacityLabel);

      ViewLabel DockingPortsLabel = new ViewLabel ("Docking Ports: " + Station.dockingPorts);
      DockingPortsLabel.setRelativeTo (Box);
      DockingPortsLabel.setTop (25);
      DockingPortsLabel.setLeft (155);
      DockingPortsLabel.setHeight (15);
      DockingPortsLabel.setWidth (150);
      DockingPortsLabel.setColor (Color.white);
      parent.Components.Add (DockingPortsLabel);

      ViewLabel DockedVesselsLabel = new ViewLabel ("Docked Vessels: " + Station.dockedVessels);
      DockedVesselsLabel.setRelativeTo (Box);
      DockedVesselsLabel.setTop (45);
      DockedVesselsLabel.setLeft (155);
      DockedVesselsLabel.setHeight (15);
      DockedVesselsLabel.setWidth (150);
      DockedVesselsLabel.setColor (Color.white);
      parent.Components.Add (DockedVesselsLabel);

      ViewLabel ScienceLabLabel = new ViewLabel ("Science Lab: " + Station.scienceLab);
      ScienceLabLabel.setRelativeTo (Box);
      ScienceLabLabel.setTop (65);
      ScienceLabLabel.setLeft (155);
      ScienceLabLabel.setHeight (15);
      ScienceLabLabel.setWidth (150);
      ScienceLabLabel.setColor (Color.white);
      parent.Components.Add (ScienceLabLabel);

      ViewLabel HasDrillLabel = new ViewLabel ("Has Drill: " + Station.drill);
      HasDrillLabel.setRelativeTo (Box);
      HasDrillLabel.setTop (85);
      HasDrillLabel.setLeft (155);
      HasDrillLabel.setHeight (15);
      HasDrillLabel.setWidth (150);
      HasDrillLabel.setColor (Color.white);
      parent.Components.Add (HasDrillLabel);

      ViewLabel AstroidLabel = new ViewLabel ("On Astroid: " + Station.onAstroid);
      AstroidLabel.setRelativeTo (Box);
      AstroidLabel.setTop (25);
      AstroidLabel.setLeft (310);
      AstroidLabel.setHeight (15);
      AstroidLabel.setWidth (150);
      AstroidLabel.setColor (Color.white);
      parent.Components.Add (AstroidLabel);

      ViewLabel SCLabel = new ViewLabel ("PO: " + Station.po);
      SCLabel.setRelativeTo (Box);
      SCLabel.setTop (45);
      SCLabel.setLeft (310);
      SCLabel.setHeight (15);
      SCLabel.setWidth (150);
      SCLabel.setColor (Color.white);
      parent.Components.Add (SCLabel);

      ViewLabel POLabel = new ViewLabel ("SC: " + Station.sc);
      POLabel.setRelativeTo (Box);
      POLabel.setTop (65);
      POLabel.setLeft (310);
      POLabel.setHeight (15);
      POLabel.setWidth (150);
      POLabel.setColor (Color.white);
      parent.Components.Add (POLabel);
    }

  }
}

