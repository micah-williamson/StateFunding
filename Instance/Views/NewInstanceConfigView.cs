using System;
using UnityEngine;
using System.Collections;

namespace StateFunding {
  public class NewInstanceConfigView: View {
    private ViewWindow Window;
    private ViewImage Image;
    private ViewLabel Label;
    private ViewButton Confirm;
    private ViewLabel GovernmentDescription;
    private ViewLabel GovernmentGameplayDescription;
    private Government SelectedGovernment;
    private Action <InstanceData> OnCreateCallback;

    public NewInstanceConfigView () {
      ViewManager.addView (this);
      createWindow ();
      createGovernmentMenu ();
    }

    private void createWindow() {
      Window = new ViewWindow ("State Funding");
      Window.setMargins (300, 100);

      Image = new ViewImage ("assets/kerbalgovernment.jpg");
      Image.setRelativeTo (Window);
      Image.setPercentWidth (100);


      Label = new ViewLabel (
        "We've been told it was in the best interest of our government to have a space program for some reason. " +
        "We're not rocket scientists, but you are. We will provide funding, make our wildest dreams come true. " +
        "Or at least just help us keep our jobs."
      );
      Label.setRelativeTo (Image);
      Label.setPercentWidth (80);
      Label.setPercentHeight (20);
      Label.setPercentLeft (10);
      Label.setPercentTop (80);
      Label.setFontSize (18);
      Label.setColor (Color.white);

      GovernmentDescription = new ViewLabel ("");
      GovernmentDescription.setRelativeTo (Image);
      GovernmentDescription.setWidth (300);
      GovernmentDescription.setHeight (Window.getHeight () - Image.getHeight () - 20);
      GovernmentDescription.setTop (Image.getHeight () + 10);
      GovernmentDescription.setLeft (120);
      GovernmentDescription.setColor (Color.white);
      GovernmentDescription.setFontSize (14);

      GovernmentGameplayDescription = new ViewLabel ("");
      GovernmentGameplayDescription.setRelativeTo (Image);
      GovernmentGameplayDescription.setWidth (300);
      GovernmentGameplayDescription.setHeight (Window.getHeight () - Image.getHeight () - 20);
      GovernmentGameplayDescription.setTop (Image.getHeight () + 10);
      GovernmentGameplayDescription.setLeft (440);
      GovernmentGameplayDescription.setColor (Color.white);
      GovernmentGameplayDescription.setFontSize (14);

      Confirm = new ViewButton ("Ok!", OnConfirm);
      Confirm.setRelativeTo (Window);
      Confirm.setWidth (100);
      Confirm.setHeight (30);
      Confirm.setRight (5);
      Confirm.setBottom (5);

      this.addComponent (Window);
      this.addComponent (Image);
      this.addComponent (Label);
      this.addComponent (GovernmentDescription);
      this.addComponent (GovernmentGameplayDescription);
      this.addComponent (Confirm);
    }

    private void createGovernmentMenu() {
      for (int i = 0; i < StateFundingGlobal.fetch.Governments.ToArray().Length; i++) {
        Government Gov = (Government)StateFundingGlobal.fetch.Governments.ToArray()[i];
        ViewGovernmentButton GovBtn = new ViewGovernmentButton (Gov, SelectGovernment);
        GovBtn.setRelativeTo (Image);
        GovBtn.setWidth (100);
        GovBtn.setHeight (30);
        GovBtn.setTop (Image.getHeight() + 10+(35*i));
        GovBtn.setLeft (10);

        this.addComponent (GovBtn);
      }

      SelectGovernment ((Government)StateFundingGlobal.fetch.Governments.ToArray () [0]);
    }

    private void SelectGovernment(Government Gov) {
      SelectedGovernment = Gov;
      GovernmentDescription.label = Gov.description;
      GovernmentGameplayDescription.label = Gov.GetGameplayDescription();
      Confirm.text = "Select " + Gov.name;
    }

    private void OnConfirm () {
      InstanceData Inst = new InstanceData ();
      Inst.Gov = SelectedGovernment;
      Inst.govName = SelectedGovernment.name;
      Inst.po = (int)SelectedGovernment.startingPO;
      Inst.sc = (int)SelectedGovernment.startingSC;
      ViewManager.removeView (this);
      OnCreateCallback (Inst);
    }

    public void OnCreate(Action <InstanceData>Callback) {
      OnCreateCallback = Callback;
    }
  }
}

