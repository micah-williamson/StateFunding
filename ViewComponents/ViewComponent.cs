using System;
using UnityEngine;

namespace StateFunding {
  public class ViewComponent {
    protected float width = -1;
    protected float height = -1;
    protected float top = -1;
    protected float left = -1;
    protected float right = -1;
    protected float bottom = -1;

    protected GUIStyle Style;
    protected Color color = Color.white;
    protected int fontSize = 12;
    protected TextAnchor textAlign = TextAnchor.UpperLeft;

    public virtual void setWidth(float width) {
      this.width = width;
    }

    public virtual void setHeight(float height) {
      this.height = height;
    }

    public virtual void setStyle(GUIStyle Style) {
      this.Style = Style;
    }

    public virtual void setTop(float top) {
      this.top = top;
    }

    public virtual void setLeft(float left) {
      this.left = left;
    }

    public virtual void setRight(float right) {
      this.right = right;
    }

    public virtual void setBottom(float bottom) {
      this.bottom = bottom;
    }

    public virtual void setColor(Color color) {
      this.color = color;
    }

    public virtual void setTextAlign(TextAnchor textAlign) {
      this.textAlign = textAlign;
    }

    public virtual void setFontSize(int fontSize) {
      this.fontSize = fontSize;
    }

    public virtual GUIStyle getGUIStyle() {
      GUIStyle style = new GUIStyle ();
      style.fontSize = fontSize;
      return style;
    }

    public virtual int getTopLeftX() {
      if (left != -1) {
        return (int)left;
      }

      return (int)(Screen.width - right - width);
    }

    public virtual int getTopLeftY() {
      if (top != -1) {
        return (int)top;
      }

      return (int)(Screen.height - bottom - height);
    }

    public virtual int getBottomRightX() {
      if (left != -1) {
        return (int)left + (int)width;
      }

      return (int)(Screen.width - right);
    }

    public virtual int getBottomRightY() {
      if (top != -1) {
        return (int)top + (int)height;
      }

      return (int)(Screen.height - top);
    }

    public virtual int getWidth() {
      return (int)width;
    }

    public virtual int getHeight() {
      return (int)height;
    }

    public virtual void paint() {
      Debug.Log ("VIEW COMPONENT PAINT");
    }
  }
}

