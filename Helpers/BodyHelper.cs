using System;

namespace StateFunding {
  public static class BodyHelper {

    public static bool ACelestialBody(string name) {
      CelestialBody[] Bodies = FlightGlobals.Bodies.ToArray ();
      for (int i = 0; i < Bodies.Length; i++) {
        CelestialBody Body = Bodies [i];
        if (Body.GetName () == name) {
          return true;
        }
      }

      return false;
    }

  }
}

