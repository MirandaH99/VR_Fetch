using UnityEngine;

public class Actor : MonoBehaviour
{
    public virtual void DestroyActor()
    {
        if (OnDestroy())
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Function called when Actor is destroyed. 
    /// if you don't want automatic destroy, override and return false
    /// </summary>
    /// <returns>bool, true (default), false (overrides)</returns>
    public virtual bool OnDestroy()
    {
        return true; 
    }

}
