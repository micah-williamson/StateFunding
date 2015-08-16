using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StateFunding {
  public static class StateFundingHubBasesView {
    public static void draw (View Vw, ViewWindow Window) {
      Window.title = "Space Stations";
      Instance GameInstance = StateFundingGlobal.fetch.GameInstance;
      Review Rev = GameInstance.ActiveReview;
      Rev.touch ();

      string Description = "Below is a list of existing Bases. Vessels that are Bases should be labeled as " +
        "such. Bases increase State Confidence as well as Public Opinion. Bases are scored by the following " +
        "criteria: Total Fuel, Total Ore, Crew, Crew Capacity, Docking Port Count, Docked Vessels (aka base modules), if it " +
        "has a science lab, and if it has a drill. Bases must be able to generate their own power. Bases on the home planet " +
        "(Kerbin in most cases) will not count.";

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

      Vessel[] Bases = VesselHelper.GetBases ();

      int labelHeight = 20;

      for (int i = 0; i < Bases.Length; i++) {
        Vessel Base = Bases [i];

        string label = Base.GetName () + " is Landed At " + Base.landedAt;

        ViewLabel BaseLabel = new ViewLabel (label);
        BaseLabel.setRelativeTo (BasesScroll);
        BaseLabel.setTop (labelHeight + (labelHeight + 5) * i);
        BaseLabel.setLeft (0);
        BaseLabel.setHeight (labelHeight);
        BaseLabel.setWidth (BasesScroll.getWidth () - 20);
        BaseLabel.setColor (Color.white);

        BasesScroll.Components.Add (BaseLabel);
      }
    }
  }
}

