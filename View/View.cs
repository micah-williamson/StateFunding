using System;
using System.Collections;
using UnityEngine;

namespace StateFunding {
  public class View: MonoBehaviour {

    private bool painting = true;
    private ViewComponent[] components = new ViewComponent[0];
    
    public View () {
      
    }

    public void addComponent(ViewComponent C) {
      if (C is RelativeViewComponent) {
        if (!((RelativeViewComponent)C).hasRelative ()) {
          Debug.LogError ("Relative View Component hasn't defined what it's relative to. Refusing to add to View.");
        }
      }

      ViewComponent[] newComponents = new ViewComponent[components.Length+1];
      for(int i = 0; i < components.Length; i++) {
        newComponents[i] = components[i];
      }
      newComponents[newComponents.Length-1] = C;

      components = newComponents;
    }

    public void removeAll() {
      this.components = new ViewComponent[0];
    }

    public void hide() {
      painting = false;
    }

    public bool isPainting() {
      return painting;
    }

    public void paint() {
      for (var i = 0; i < components.Length; i++) {
        ViewComponent C = components [i];
        C.paint ();
      }
    }

    public string toString() {
      return "View";
    }

    public void show() {
      painting = true;
    }

  }
}

