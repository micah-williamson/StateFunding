using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StateFunding {
  public static class StateFundingHubKerbalsView {
    public static void draw (View Vw, ViewWindow Window) {
      Window.title = "Kerbals";
      InstanceData GameInstance = StateFundingGlobal.fetch.GameInstance;
      Review Rev = GameInstance.ActiveReview;
      Rev.touch ();

      string Description = "You Love Kerbals, I Love Kerbals, Kerbals Love Kerbals. Just one of those facts of life. " +
        "So it goes without saying, having Kerbals actively on missions increases Public Opinion. " +
        "The more Kerbals you have in flight the more Public Opinion you will garner, but be careful, " +
        "a stranded Kerbal is as bad as a dead Kerbal and will hurt public opinion until they are " +
        "rescued. A qualified \"Stranded Kerbal\" is one that is in a vessel without fuel/energy, a science lab, " +
        "or a mining rig. They are floating without reason to be there. A kerbal will not be considered stranded unless it's " +
        "been on the current mission for at least 2 years.";

      ViewLabel DescriptionLabel = new ViewLabel (Description);
      DescriptionLabel.setRelativeTo (Window);
      DescriptionLabel.setLeft (140);
      DescriptionLabel.setTop (20);
      DescriptionLabel.setColor (Color.white);
      DescriptionLabel.setHeight (100);
      DescriptionLabel.setWidth (Window.getWidth () - 140);

      Vw.addComponent (DescriptionLabel);

      ViewLabel ActiveKerbals = new ViewLabel ("Active Kerbals: " + Rev.activeKerbals + ". Stranded Kerbals: " + Rev.strandedKerbals + ".");
      ActiveKerbals.setRelativeTo (Window);
      ActiveKerbals.setLeft (140);
      ActiveKerbals.setTop (130);
      ActiveKerbals.setColor (Color.white);
      ActiveKerbals.setHeight (30);
      ActiveKerbals.setWidth (Window.getWidth () - 140);

      Vw.addComponent (ActiveKerbals);

      ViewScroll KerbalsScroll = new ViewScroll ();
      KerbalsScroll.setRelativeTo (Window);
      KerbalsScroll.setWidth (Window.getWidth () - 140);
      KerbalsScroll.setHeight (Window.getHeight () - 160);
      KerbalsScroll.setLeft (140);
      KerbalsScroll.setTop (150);

      Vw.addComponent (KerbalsScroll);

      ProtoCrewMember[] Kerbals = KerbalHelper.GetKerbals ();

      int labelHeight = 20;

      for (int i = 0; i < Kerbals.Length; i++) {
        ProtoCrewMember Kerb = Kerbals [i];

        string state = "Active";
        Color color = Color.green;
        if (KerbalHelper.IsStranded (Kerb)) {
          state = "Stranded";
          color = Color.white;
        } else if (KerbalHelper.QualifiedStranded(Kerb)) {
          state = "Active [Will be Stranded In " + KerbalHelper.TimeToStranded (Kerb) + " Days!]";
          color = Color.yellow;
        }

        string label = Kerb.name + " (" + state + ")";

        ViewLabel KerbalLabel = new ViewLabel (label);
        KerbalLabel.setRelativeTo (KerbalsScroll);
        KerbalLabel.setTop (labelHeight + (labelHeight + 5) * i);
        KerbalLabel.setLeft (0);
        KerbalLabel.setHeight (labelHeight);
        KerbalLabel.setWidth (KerbalsScroll.getWidth() - 20);
        KerbalLabel.setColor (color);
        KerbalsScroll.Components.Add (KerbalLabel);
      }

    }
  }
}

