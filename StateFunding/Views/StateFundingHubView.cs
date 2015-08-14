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
      Window.setMargins (300, 100);

      this.addComponent (Window);

      SideMenu.Add(new ViewButton("Current State", LoadCurrentState));
      SideMenu.Add(new ViewButton("Sat Coverage", LoadSatelliteCoverage));
      SideMenu.Add(new ViewButton("Science Stations", LoadScienceStations));
      SideMenu.Add(new ViewButton("Mining Rigs", LoadMiningRigs));
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

      Window.title = "Satellite Coverage";
      Instance GameInstance = StateFundingGlobal.fetch.GameInstance;
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

      this.addComponent (DescriptionLabel);

      ViewLabel TotalCoverage = new ViewLabel ("Total Coverage: " + Math.Round ((double)Rev.satelliteCoverage * 100) + "%");
      TotalCoverage.setRelativeTo (Window);
      TotalCoverage.setLeft (140);
      TotalCoverage.setTop (130);
      TotalCoverage.setColor (Color.white);
      TotalCoverage.setHeight (30);
      TotalCoverage.setWidth (Window.getWidth () - 140);

      this.addComponent (TotalCoverage);

      ViewScroll CoverageScroll = new ViewScroll ();
      CoverageScroll.setRelativeTo (Window);
      CoverageScroll.setWidth (Window.getWidth () - 140);
      CoverageScroll.setHeight (Window.getHeight () - 160);
      CoverageScroll.setLeft (140);
      CoverageScroll.setTop (150);

      this.addComponent (CoverageScroll);

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

    private void LoadScienceStations() {
      reloadBase ();

      Window.title = "Science Stations";
      Instance GameInstance = StateFundingGlobal.fetch.GameInstance;
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

      this.addComponent (DescriptionLabel);

      ViewLabel TotalCoverage = new ViewLabel ("Orbiting Stations: " + Rev.orbitalScienceStations + ". " +
                                               "Landed Stations: " + Rev.planetaryScienceStations + ".");
      TotalCoverage.setRelativeTo (Window);
      TotalCoverage.setLeft (140);
      TotalCoverage.setTop (130);
      TotalCoverage.setColor (Color.white);
      TotalCoverage.setHeight (30);
      TotalCoverage.setWidth (Window.getWidth () - 140);

      this.addComponent (TotalCoverage);

      ViewScroll StationsScroll = new ViewScroll ();
      StationsScroll.setRelativeTo (Window);
      StationsScroll.setWidth (Window.getWidth () - 140);
      StationsScroll.setHeight (Window.getHeight () - 160);
      StationsScroll.setLeft (140);
      StationsScroll.setTop (150);

      this.addComponent(StationsScroll);

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

    private void LoadMiningRigs() {
      reloadBase ();

      Window.title = "Mining Rigs";
      Instance GameInstance = StateFundingGlobal.fetch.GameInstance;
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

      this.addComponent (DescriptionLabel);

      ViewLabel TotalCoverage = new ViewLabel ("Mining Rigs: " + Rev.miningRigs);
      TotalCoverage.setRelativeTo (Window);
      TotalCoverage.setLeft (140);
      TotalCoverage.setTop (130);
      TotalCoverage.setColor (Color.white);
      TotalCoverage.setHeight (30);
      TotalCoverage.setWidth (Window.getWidth () - 140);

      this.addComponent (TotalCoverage);

      ViewScroll RigsScroll = new ViewScroll ();
      RigsScroll.setRelativeTo (Window);
      RigsScroll.setWidth (Window.getWidth () - 140);
      RigsScroll.setHeight (Window.getHeight () - 160);
      RigsScroll.setLeft (140);
      RigsScroll.setTop (150);

      this.addComponent (RigsScroll);

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

    private void LoadKerbals() {
      reloadBase ();

      Window.title = "Kerbals";
      Instance GameInstance = StateFundingGlobal.fetch.GameInstance;
      Review Rev = GameInstance.ActiveReview;
      Rev.touch ();

      string Description = "You Love Kerbals, I Love Kerbals, Kerbals Love Kerbals. Just one of those facts of life. " +
                           "So it goes without saying, having Kerbals actively on missions increases Public Opinion. " +
                           "The more Kerbals you have in flight the more Public Opinion you will garner, but be careful, " +
                           "a stranded Kerbal is as bad as a dead Kerbal and will hurt public opinion until they are " +
                           "rescued. A qualified \"Stranded Kerbal\" is one that is in a vessel without fuel, a science lab, " +
                           "or a mining rig. They are floating without reason to be there.";

      ViewLabel DescriptionLabel = new ViewLabel (Description);
      DescriptionLabel.setRelativeTo (Window);
      DescriptionLabel.setLeft (140);
      DescriptionLabel.setTop (20);
      DescriptionLabel.setColor (Color.white);
      DescriptionLabel.setHeight (100);
      DescriptionLabel.setWidth (Window.getWidth () - 140);

      this.addComponent (DescriptionLabel);

      ViewLabel ActiveKerbals = new ViewLabel ("Active Kerbals: " + Rev.activeKerbals + ". Stranded Kerbals: " + Rev.strandedKerbals + ".");
      ActiveKerbals.setRelativeTo (Window);
      ActiveKerbals.setLeft (140);
      ActiveKerbals.setTop (130);
      ActiveKerbals.setColor (Color.white);
      ActiveKerbals.setHeight (30);
      ActiveKerbals.setWidth (Window.getWidth () - 140);

      this.addComponent (ActiveKerbals);

      ViewScroll KerbalsScroll = new ViewScroll ();
      KerbalsScroll.setRelativeTo (Window);
      KerbalsScroll.setWidth (Window.getWidth () - 140);
      KerbalsScroll.setHeight (Window.getHeight () - 160);
      KerbalsScroll.setLeft (140);
      KerbalsScroll.setTop (150);

      this.addComponent (KerbalsScroll);

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

    private void LoadCurrentState() {
      reloadBase ();

      Window.title = "Current State";
      Instance GameInstance = StateFundingGlobal.fetch.GameInstance;
      Review Rev = GameInstance.ActiveReview;
      Rev.touch ();

      ViewTextArea TextArea = new ViewTextArea (GameInstance.ActiveReview.GetText());
      TextArea.setRelativeTo (Window);
      TextArea.setTop (40);
      TextArea.setLeft (130);
      TextArea.setWidth (Window.getWidth () - 140);
      TextArea.setHeight (Window.getHeight () - 40);
      TextArea.setColor (Color.white);

      this.addComponent (TextArea);
    }

    private void LoadPastReviews() {
      reloadBase ();

      Window.title = "Past Reviews";
      Instance GameInstance = StateFundingGlobal.fetch.GameInstance;

      int buttonWidth = 180;
      int buttonHeight = 30;
      int buttonMargin = 10;
      int xOffset = 0;
      int yOffset = 0;

      ViewScroll PastReviewsScroll = new ViewScroll ();
      PastReviewsScroll.setRelativeTo (Window);
      PastReviewsScroll.setWidth (Window.getWidth () - 140);
      PastReviewsScroll.setHeight (Window.getHeight () - 50);
      PastReviewsScroll.setLeft (140);
      PastReviewsScroll.setTop (40);

      this.addComponent (PastReviewsScroll);

      for (int i = GameInstance.getReviews ().Length - 1; i >= 0; i--) {
        Review Rev = GameInstance.getReviews () [i];

        ViewReviewButton Btn = new ViewReviewButton (Rev, OnReviewClick);
        Btn.setRelativeTo (PastReviewsScroll);

        int left = 0;
        int top = yOffset * buttonMargin + yOffset * buttonHeight;

        Btn.setLeft (left);
        Btn.setTop (top);
        Btn.setWidth (buttonWidth);
        Btn.setHeight (buttonHeight);
        Btn.setColor (Color.white);

        yOffset++;

        PastReviewsScroll.Components.Add (Btn);
      }
    }

    private void LoadGuide() {
      reloadBase ();

      Window.title = "StateFunding Guide";

      string guideLabel = "StateFunding Version 0.1.0\n" +
                          "--------------------------\n\n" +
                          "[*] Overview\n" +
                          "StateFunding offers another path to funding your space program. When you chose your government " +
                          "you chose based on the strengths and weaknesses of that government. If you government cares more " +
                          "more about public opinion, using Kerbals will have a greater inpact on your funding but you will also " +
                          "be penalized for Kerbal death more. If your government cares more about their own confidence in your " +
                          "program, using autonomous rockets is a safer, cheaper, and more affective. Each year you will recieve " +
                          "a year review showing the achievements and failures and you will be paid by your total PO/SC scores.\n\n" +
                          "[*] Public Opinion (PO)\n" +
                          "To raise public opinion, have more Kerbals in active flights. The amount of science generated from " +
                          "science stations will also increase public opinion. Kerbal death will negatively impact public opinion\n\n" +
                          "[*] State Confidence (SC)\n" +
                          "To raise state confidence, have satelites with antenas in orbit of celestial bodies other than the Sun. " +
                          "You can have 10 satelites in orbit of each celestial body affect your SC score. Resource mining will " +
                          "also increase your SC score. Destroyed vessels will have a negative impact on the SC score.";

      ViewTextArea TextArea = new ViewTextArea (guideLabel);
      TextArea.setRelativeTo (Window);
      TextArea.setTop (40);
      TextArea.setLeft (130);
      TextArea.setWidth (Window.getWidth () - 140);
      TextArea.setHeight (Window.getHeight () - 40);
      TextArea.setColor (Color.white);

      this.addComponent (TextArea);
    }

    public void OnReviewClick(Review Rev) {
      StateFundingGlobal.fetch.ReviewMgr.OpenReview(Rev);
    }
  }
}

