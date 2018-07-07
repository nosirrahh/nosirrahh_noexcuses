using UnityEngine;

public class Health : MonoBehaviour
{
    #region Classes

    [System.Serializable]
    public class HealthEvent
    {
        public float previousHealth;
        public float currentHealth;
        public Health healthComponent;

        public HealthEvent (float previousHealth, float currentHealth, Health healthComponent)
        {
            this.previousHealth = previousHealth;
            this.currentHealth = currentHealth;
            this.healthComponent = healthComponent;
        }
    }

    #endregion

    #region Events

    public event System.Action<HealthEvent> OnHealthChange;
    public event System.Action<HealthEvent> OnAddHealth;
    public event System.Action<HealthEvent> OnRemoveHealth;

    #endregion

    #region Fields

    [SerializeField]
    private ulong maxHealth;
    private float currentHealth;
    private float previousHealth;

    #endregion

    #region Properties

    public ulong MaxHealth { get { return maxHealth; } }
    public float CurrentHealth { get { return currentHealth; } }
    public float PreviousHealth { get { return previousHealth; } }
    public float PercentHealth { get { return currentHealth / maxHealth; } }

    #endregion

    #region Unity Methods

    private void Awake ()
    {
        AddHealth (maxHealth);
    }

    #endregion

    #region Public Methods

    public void AddHealth (float healthToAdd)
    {
        HealthEvent hEvent = ChangeHealth (Mathf.Abs (healthToAdd));
        if (OnAddHealth != null)
            OnAddHealth (hEvent);
    }

    public void RemoveHealth (float healthToRemove)
    {
        HealthEvent hEvent = ChangeHealth (-Mathf.Abs (healthToRemove));
        if (OnRemoveHealth != null)
            OnAddHealth (hEvent);
    }

    #endregion

    #region Private Methods

    private HealthEvent ChangeHealth (float h)
    {
        previousHealth = currentHealth;
        currentHealth = Mathf.Clamp (currentHealth + h, 0, maxHealth);
        HealthEvent hEvent = new HealthEvent (previousHealth, currentHealth, this);
        if (OnHealthChange != null)
            OnHealthChange (hEvent);
        return hEvent;
    }

    #endregion
}
