using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StateFunding {
  public static class StateFundingHubCoverageView {
    public static void draw (View Vw, ViewWindow Window) {
      Window.title = "Satellite Coverage";
      InstanceData GameInstance = StateFundingGlobal.fetch.GameInstance;
      Review Rev = GameInstance.ActiveReview;
      Rev.touch ();

      string Description = "Below is your space programs satellite coverage. Satellite coverage increases State Confidence. " +
        "The number of satellites needed to provide full coverage veries depending on the size of the " +
        "celestial body. Kerbin needs 10 satelites to be fully covered while a small moon like Pol only " +
        "needs 1 and the massive Jool needs 100. Your total coverage is calculated on the coverage provided " +
        "to all celestial bodies. So even though Jool needs so many you can still get a high coverage " +
        "rating by covering the smaller bodies. So start with Kerbin, moons, and the near planets. To have " +
        "a qualified \"Surveyor Satellite\" it must have an antenna, an autonomous control system, and be " +
        "able to generate power.";

      ViewLabel DescriptionLabel = new ViewLabel (Description);
      DescriptionLabel.setRelativeTo (Window);
      DescriptionLabel.setLeft (140);
      DescriptionLabel.setTop (20);
      DescriptionLabel.setColor (Color.white);
      DescriptionLabel.setHeight (100);
      DescriptionLabel.setWidth (Window.getWidth () - 140);

      Vw.addComponent (DescriptionLabel);

      ViewLabel TotalCoverage = new ViewLabel ("Total Coverage: " + Math.Round ((double)Rev.satelliteCoverage * 100) + "%");
      TotalCoverage.setRelativeTo (Window);
      TotalCoverage.setLeft (140);
      TotalCoverage.setTop (130);
      TotalCoverage.setColor (Color.white);
      TotalCoverage.setHeight (30);
      TotalCoverage.setWidth (Window.getWidth () - 140);

      Vw.addComponent (TotalCoverage);

      ViewScroll CoverageScroll = new ViewScroll ();
      CoverageScroll.setRelativeTo (Window);
      CoverageScroll.setWidth (Window.getWidth () - 140);
      CoverageScroll.setHeight (Window.getHeight () - 160);
      CoverageScroll.setLeft (140);
      CoverageScroll.setTop (150);

      Vw.addComponent (CoverageScroll);

      CoverageReport[] Coverages = Rev.Coverages;

      int labelHeight = 20;

      for (int i = 0; i < Coverages.Length; i++) {
        CoverageReport Coverage = Coverages [i];
        string label = Coverage.entity + " : (" +
          Coverage.satCount + "/" +
          Coverage.satCountForFullCoverage + ") " +
          Math.Round (Coverage.coverage * 100) + "%";

        ViewLabel CoverageLabel = new ViewLabel (label);
        CoverageLabel.setRelativeTo (CoverageScroll);
        CoverageLabel.setTop (labelHeight + (labelHeight + 5) * i);
        CoverageLabel.setLeft (0);
        CoverageLabel.setHeight (labelHeight);
        CoverageLabel.setWidth (CoverageScroll.getWidth () - 20);

        if (Coverage.coverage <= 0.25) {
          CoverageLabel.setColor (Color.white);
        } else if (Coverage.coverage <= .75) {
          CoverageLabel.setColor (Color.yellow);
        } else {
          CoverageLabel.setColor (Color.green);
        }

        CoverageScroll.Components.Add (CoverageLabel);
      }
    }
  }
}

