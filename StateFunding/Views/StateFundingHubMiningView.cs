using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StateFunding {
  public static class StateFundingHubMiningView {
    public static void draw (View Vw, ViewWindow Window) {
      Window.title = "Mining Rigs";
      InstanceData GameInstance = StateFundingGlobal.fetch.GameInstance;
      Review Rev = GameInstance.ActiveReview;
      Rev.touch ();

      string Description = "Below is a list of existing Mining Rigs. Having more Mining Rigs increases State " +
        "Confidence. To have a qualified Mining Rig is must have an antenna, drill, be able to generate power, " +
        "and be Landed on a body other than Kerbin.";

      ViewLabel DescriptionLabel = new ViewLabel (Description);
      DescriptionLabel.setRelativeTo (Window);
      DescriptionLabel.setLeft (140);
      DescriptionLabel.setTop (20);
      DescriptionLabel.setColor (Color.white);
      DescriptionLabel.setHeight (100);
      DescriptionLabel.setWidth (Window.getWidth () - 140);

      Vw.addComponent (DescriptionLabel);

      ViewLabel TotalCoverage = new ViewLabel ("Mining Rigs: " + Rev.miningRigs);
      TotalCoverage.setRelativeTo (Window);
      TotalCoverage.setLeft (140);
      TotalCoverage.setTop (130);
      TotalCoverage.setColor (Color.white);
      TotalCoverage.setHeight (30);
      TotalCoverage.setWidth (Window.getWidth () - 140);

      Vw.addComponent (TotalCoverage);

      ViewScroll RigsScroll = new ViewScroll ();
      RigsScroll.setRelativeTo (Window);
      RigsScroll.setWidth (Window.getWidth () - 140);
      RigsScroll.setHeight (Window.getHeight () - 160);
      RigsScroll.setLeft (140);
      RigsScroll.setTop (150);

      Vw.addComponent (RigsScroll);

      Vessel[] MiningRigs = VesselHelper.GetMiningRigs ();

      int labelHeight = 20;

      for (int i = 0; i < MiningRigs.Length; i++) {
        Vessel MiningRig = MiningRigs [i];

        string label = MiningRig.GetName () + " is Landed At " + MiningRig.mainBody.GetName ();;

        ViewLabel MiningLabel = new ViewLabel (label);
        MiningLabel.setRelativeTo (RigsScroll);
        MiningLabel.setTop (labelHeight + (labelHeight + 5) * i);
        MiningLabel.setLeft (0);
        MiningLabel.setHeight (labelHeight);
        MiningLabel.setWidth (RigsScroll.getWidth () - 20);
        MiningLabel.setColor (Color.white);

        RigsScroll.Components.Add (MiningLabel);
      }
    }
  }
}

