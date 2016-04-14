using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StateFunding {
  public static class StateFundingHubBasesView {
    public static void draw (View Vw, ViewWindow Window) {
      Window.title = "Bases";
      InstanceData GameInstance = StateFundingGlobal.fetch.GameInstance;
      Review Rev = GameInstance.ActiveReview;
      Rev.touch ();

      string Description = "Below is a list of existing Bases. Vessels that are Bases should be labeled as " +
                           "such, be landed on a body other than the home planet, and be able to generate power. Bases increase State Confidence " +
                           "as well as Public Opinion. Bases are scored by the following criteria: Total Fuel (SC), Total Ore (SC), Crew (PO), Crew Capacity " +
                           "(SC), Docking Port Count (SC), Docked Vessels (PO), if it has a science lab (SC/PO), and if it has a drill (SC/PO).";
      
      ViewLabel DescriptionLabel = new ViewLabel (Description);
      DescriptionLabel.setRelativeTo (Window);
      DescriptionLabel.setLeft (140);
      DescriptionLabel.setTop (20);
      DescriptionLabel.setColor (Color.white);
      DescriptionLabel.setHeight (100);
      DescriptionLabel.setWidth (Window.getWidth () - 140);

      Vw.addComponent (DescriptionLabel);

      ViewLabel TotalBases = new ViewLabel ("Total Bases: " + Rev.Bases.Length);
      TotalBases.setRelativeTo (Window);
      TotalBases.setLeft (140);
      TotalBases.setTop (130);
      TotalBases.setColor (Color.white);
      TotalBases.setHeight (30);
      TotalBases.setWidth (Window.getWidth () - 140);

      Vw.addComponent (TotalBases);

      ViewScroll BasesScroll = new ViewScroll ();
      BasesScroll.setRelativeTo (Window);
      BasesScroll.setWidth (Window.getWidth () - 140);
      BasesScroll.setHeight (Window.getHeight () - 160);
      BasesScroll.setLeft (140);
      BasesScroll.setTop (150);

      Vw.addComponent(BasesScroll);

      BaseReport[] Bases = Rev.Bases;

      for (int i = 0; i < Bases.Length; i++) {
        drawItem (Bases [i], BasesScroll, i);
      }

    }

    public static void drawItem(BaseReport Base, ViewScroll parent, int offset) {
      int boxHeight = 110;

      ViewBox Box = new ViewBox ();
      Box.setRelativeTo (parent);
      Box.setWidth (parent.getWidth () - 20);
      Box.setHeight (boxHeight);
      Box.setLeft (0);
      Box.setTop ((boxHeight + 10) * offset);
      Box.setColor (Color.white);
      parent.Components.Add (Box);

      string label = "[" + Base.name + " is Landed At " + Base.entity + "]";
      ViewLabel BaseLabel = new ViewLabel (label);
      BaseLabel.setRelativeTo (Box);
      BaseLabel.setTop (5);
      BaseLabel.setLeft (5);
      BaseLabel.setHeight (15);
      BaseLabel.setPercentWidth (100);
      BaseLabel.setColor (Color.green);
      parent.Components.Add (BaseLabel);

      ViewLabel FuelLabel = new ViewLabel ("Fuel: " + Base.fuel);
      FuelLabel.setRelativeTo (Box);
      FuelLabel.setTop (25);
      FuelLabel.setLeft (5);
      FuelLabel.setHeight (15);
      FuelLabel.setWidth (150);
      FuelLabel.setColor (Color.white);
      parent.Components.Add (FuelLabel);

      ViewLabel OreLabel = new ViewLabel ("Ore: " + Base.ore);
      OreLabel.setRelativeTo (Box);
      OreLabel.setTop (45);
      OreLabel.setLeft (5);
      OreLabel.setHeight (20);
      OreLabel.setWidth (150);
      OreLabel.setColor (Color.white);
      parent.Components.Add (OreLabel);

      ViewLabel CrewLabel = new ViewLabel ("Crew: " + Base.crew);
      CrewLabel.setRelativeTo (Box);
      CrewLabel.setTop (65);
      CrewLabel.setLeft (5);
      CrewLabel.setHeight (20);
      CrewLabel.setWidth (150);
      CrewLabel.setColor (Color.white);
      parent.Components.Add (CrewLabel);

      ViewLabel CrewCapacityLabel = new ViewLabel ("Crew Capacity: " + Base.crewCapacity);
      CrewCapacityLabel.setRelativeTo (Box);
      CrewCapacityLabel.setTop (85);
      CrewCapacityLabel.setLeft (5);
      CrewCapacityLabel.setHeight (20);
      CrewCapacityLabel.setWidth (150);
      CrewCapacityLabel.setColor (Color.white);
      parent.Components.Add (CrewCapacityLabel);

      ViewLabel DockingPortsLabel = new ViewLabel ("Docking Ports: " + Base.dockingPorts);
      DockingPortsLabel.setRelativeTo (Box);
      DockingPortsLabel.setTop (25);
      DockingPortsLabel.setLeft (155);
      DockingPortsLabel.setHeight (15);
      DockingPortsLabel.setWidth (150);
      DockingPortsLabel.setColor (Color.white);
      parent.Components.Add (DockingPortsLabel);

      ViewLabel DockedVesselsLabel = new ViewLabel ("Docked Vessels: " + Base.dockedVessels);
      DockedVesselsLabel.setRelativeTo (Box);
      DockedVesselsLabel.setTop (45);
      DockedVesselsLabel.setLeft (155);
      DockedVesselsLabel.setHeight (15);
      DockedVesselsLabel.setWidth (150);
      DockedVesselsLabel.setColor (Color.white);
      parent.Components.Add (DockedVesselsLabel);

      ViewLabel ScienceLabLabel = new ViewLabel ("Science Lab: " + Base.scienceLab);
      ScienceLabLabel.setRelativeTo (Box);
      ScienceLabLabel.setTop (65);
      ScienceLabLabel.setLeft (155);
      ScienceLabLabel.setHeight (15);
      ScienceLabLabel.setWidth (150);
      ScienceLabLabel.setColor (Color.white);
      parent.Components.Add (ScienceLabLabel);

      ViewLabel HasDrillLabel = new ViewLabel ("Has Drill: " + Base.drill);
      HasDrillLabel.setRelativeTo (Box);
      HasDrillLabel.setTop (85);
      HasDrillLabel.setLeft (155);
      HasDrillLabel.setHeight (15);
      HasDrillLabel.setWidth (150);
      HasDrillLabel.setColor (Color.white);
      parent.Components.Add (HasDrillLabel);

      ViewLabel SCLabel = new ViewLabel ("PO: " + Base.po);
      SCLabel.setRelativeTo (Box);
      SCLabel.setTop (25);
      SCLabel.setLeft (310);
      SCLabel.setHeight (15);
      SCLabel.setWidth (150);
      SCLabel.setColor (Color.white);
      parent.Components.Add (SCLabel);

      ViewLabel POLabel = new ViewLabel ("SC: " + Base.sc);
      POLabel.setRelativeTo (Box);
      POLabel.setTop (45);
      POLabel.setLeft (310);
      POLabel.setHeight (15);
      POLabel.setWidth (150);
      POLabel.setColor (Color.white);
      parent.Components.Add (POLabel);
    }
  }
}

