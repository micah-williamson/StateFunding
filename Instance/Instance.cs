using System;

namespace StateFunding {
  public class Instance  {
    public Government Gov;

    [KSPField (isPersistant=true)]
    public Review ActiveReview;

    [KSPField (isPersistant=true)]
    public string govName;

    [KSPField (isPersistant=true)]
    public int po;

    [KSPField (isPersistant=true)]
    public int sc;

    [KSPField (isPersistant=true)]
    Review[] Reviews = new Review[0];

    public Instance () {
      ActiveReview = new Review ();
    }

    public void addReview (Review R) {
      Review[] NewReviews = new Review[Reviews.Length+1];
      for(int i = 0; i < Reviews.Length; i++) {
        NewReviews[i] = Reviews[i];
      }
      NewReviews[NewReviews.Length-1] = R;

      Reviews = NewReviews;
    }

    public Review[] getReviews() {
      return Reviews;
    }
  }
}

