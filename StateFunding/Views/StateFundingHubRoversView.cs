using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StateFunding {
  public static class StateFundingHubRoversView {
    public static void draw (View Vw, ViewWindow Window) {
      Window.title = "Rovers";
      InstanceData GameInstance = StateFundingGlobal.fetch.GameInstance;
      Review Rev = GameInstance.ActiveReview;
      Rev.touch ();

      string Description = "Below is a list of existing Rovers. Having more Rovers increases Public Opinion." +
        "Vessels that are rovers should be labeled as a Rover. They should have at least 4 wheels but can have more." +
        "If any wheels on the rover are broken they must be repaired. Rovers must has energy and be landed on a body other " +
        "than the home planet (Kerbin in most cases) to count.";

      ViewLabel DescriptionLabel = new ViewLabel (Description);
      DescriptionLabel.setRelativeTo (Window);
      DescriptionLabel.setLeft (140);
      DescriptionLabel.setTop (20);
      DescriptionLabel.setColor (Color.white);
      DescriptionLabel.setHeight (100);
      DescriptionLabel.setWidth (Window.getWidth () - 140);

      Vw.addComponent (DescriptionLabel);

      ViewLabel TotalRovers = new ViewLabel ("Total Rovers: " + Rev.rovers);
      TotalRovers.setRelativeTo (Window);
      TotalRovers.setLeft (140);
      TotalRovers.setTop (130);
      TotalRovers.setColor (Color.white);
      TotalRovers.setHeight (30);
      TotalRovers.setWidth (Window.getWidth () - 140);

      Vw.addComponent (TotalRovers);

      ViewScroll RoversScroll = new ViewScroll ();
      RoversScroll.setRelativeTo (Window);
      RoversScroll.setWidth (Window.getWidth () - 140);
      RoversScroll.setHeight (Window.getHeight () - 160);
      RoversScroll.setLeft (140);
      RoversScroll.setTop (150);

      Vw.addComponent(RoversScroll);

      Vessel[] Rovers = VesselHelper.GetRovers ();

      int labelHeight = 20;

      for (int i = 0; i < Rovers.Length; i++) {
        Vessel Rover = Rovers [i];
        string target;

        string label = Rover.GetName () + " is Landed at " + Rover.mainBody.GetName ();

        ViewLabel RoverLabel = new ViewLabel (label);
        RoverLabel.setRelativeTo (RoversScroll);
        RoverLabel.setTop (labelHeight + (labelHeight + 5) * i);
        RoverLabel.setLeft (0);
        RoverLabel.setHeight (labelHeight);
        RoverLabel.setWidth (RoversScroll.getWidth () - 20);
        RoverLabel.setColor (Color.white);

        RoversScroll.Components.Add (RoverLabel);
      }

    }
  }
}

