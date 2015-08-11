using System;
using UnityEngine;
using System.IO;

namespace StateFunding {
  public class ViewImage: RelativeViewComponent {

    protected string path;
    protected Texture2D Image;

    public ViewImage (string path) {
      this.path = path;
      Image = new Texture2D (2, 2);
      Image.LoadImage (File.ReadAllBytes ("GameData/StateFunding/" + path));
    }

    public override int getWidth () {
      /* calc auto width */
      if (width == -1 && percentWidth == -1) {
        float scale = (float)Image.width / (float)Image.height;
        return (int)(getHeight () * scale);
      }

      return base.getWidth ();
    }

    public override int getHeight () {
      /* calc auto width */
      if (height == -1 && percentHeight == -1) {
        float scale = (float)Image.height / (float)Image.width;
        return (int)(getWidth () * scale);
      }

      return base.getHeight ();
    }

    public override void paint() {
      GUI.Box (new Rect (
        this.getTopLeftX (),
        this.getTopLeftY (),
        this.getWidth (),
        this.getHeight ()), Image);
    }
  }
}