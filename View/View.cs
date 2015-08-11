using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateFunding {
  public class View: MonoBehaviour {

    private bool painting = true;
    private List<ViewComponent> Components = new List<ViewComponent>();
    
    public View () {}

    public void addComponent(ViewComponent C) {
      if (C is RelativeViewComponent) {
        if (!((RelativeViewComponent)C).hasRelative ()) {
          Debug.LogError ("Relative View Component hasn't defined what it's relative to. Refusing to add to View.");
        }
      }

      Components.Add (C);
    }

    public void removeAll() {
      Components.Clear();
    }

    public void hide() {
      painting = false;
    }

    public bool isPainting() {
      return painting;
    }

    public void paint() {
      for (var i = 0; i < Components.ToArray().Length; i++) {
        ViewComponent C = Components.ToArray() [i];
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

