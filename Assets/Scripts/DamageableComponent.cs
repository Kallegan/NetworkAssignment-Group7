using Alteruna;
using System.Collections;
using UnityEngine;

public class DamageableComponent : AttributesSync
{
    private Alteruna.Avatar avatar;
    [SerializeField] Transform HealthBar;
    [SerializeField] ParticleSystem StunEmitter;
    private ParticleSystem StunEffect;

    [SerializeField] private float WorldDamageImmunityTime = 0.5f;
    [SerializeField] private int WorldDamage = 1;
    [SerializeField] private float MaxHealth = 10;
    [SynchronizableField] private float Health = 10;

    private PlayerMovement PlayerMovement;
    private Quaternion stunVFXRotation;
    private Vector3 stunVFXOffset;

    private Camera cam;
    private bool RecentlyDamaged = false;
   

    private void Awake()
    {
        cam = Camera.main;
        PlayerMovement = transform.parent.GetComponent<PlayerMovement>();
        avatar = gameObject.GetComponentInParent(typeof(Alteruna.Avatar)) as Alteruna.Avatar;
        stunVFXRotation = new Quaternion(70, 90, 90, 0);
        stunVFXOffset = new Vector3(0, 2, 0);

    }

    private void Start()
    {
        StunEffect = Instantiate(StunEmitter, transform.position + stunVFXOffset, stunVFXRotation);
        StunEffect.Stop();
        if (!avatar.IsMe)
            return;
        Health = MaxHealth;        
    }

    // Update is called once per frame
    void Update()
    {
        //Update size of healthbar
        Vector3 healthScale = new Vector3((Health / MaxHealth) * 2, HealthBar.localScale.y, HealthBar.localScale.z);
        HealthBar.localScale = healthScale;

        //turns healthbars to player camera.
        HealthBar.transform.LookAt(HealthBar.transform.position + cam.transform.rotation * Vector3.forward);

        if (WorldManager.Instance.TakeWorldDamage(transform.parent.position) && !RecentlyDamaged)
            TakeWorldDamage();

                   
        StunEffect.transform.position = transform.position + stunVFXOffset;
              
            
    }

    

    private void TakeWorldDamage()
    {
        if (RecentlyDamaged)
            return;

        StartCoroutine(TickDamage());
    }

    private IEnumerator TickDamage()
    {
        RecentlyDamaged = true;

        yield return new WaitForSeconds(WorldDamageImmunityTime);

        RecentlyDamaged = false;
        OnHit(WorldDamage);
    }

    public void OnHit(int damageAmount)
    {
        TakeDamage(damageAmount);        
    }
    
    public void OnHit(int damageAmount, Vector3 knockbackDirection)
    {
        TakeDamage(damageAmount);
        // Todo: deal with stuntime in a better way
        PlayerMovement.SetAsStunned(0.5f);       


        //PlayerMovement.velocity = knockbackDirection.normalized;
    }
    void TakeDamage(int damageAmount)
    {
        Health -= damageAmount;
        BroadcastRemoteMethod("UpdateHealthBar");
        BroadcastRemoteMethod("HitVFX");
        if (Health <= 0)
        {
            BroadcastRemoteMethod("Die");
        }
    }

    [SynchronizableMethod]
    void UpdateHealthBar()
    {
        if (!avatar.IsMe)
            return;
        Vector3 healthScale = new Vector3((Health / MaxHealth) * 2, HealthBar.localScale.y, HealthBar.localScale.z);
        HealthBar.localScale = healthScale;
    }

    [SynchronizableMethod]
    private void Die()
    {
        transform.parent.GetComponentInChildren<PlayerStateSync>().isAlive = false;
        Destroy(transform.parent.gameObject); //temp remove when we have something cooler       
    }

    [SynchronizableMethod]
    private void HitVFX()
    {
        StunEffect.Play();
    }
}
/*
 * According to all known laws
of aviation,

  
there is no way a bee
should be able to fly.

  
Its wings are too small to get
its fat little body off the ground.

  
The bee, of course, flies anyway

  
because bees don't care
what humans think is impossible.

  
Yellow, black. Yellow, black.
Yellow, black. Yellow, black.

  
Ooh, black and yellow!
Let's shake it up a little.

  
Barry! Breakfast is ready!

  
Ooming!

  
Hang on a second.

  
Hello?

  
- Barry?
- Adam?

  
- Oan you believe this is happening?
- I can't. I'll pick you up.

  
Looking sharp.

  
Use the stairs. Your father
paid good money for those.

  
Sorry. I'm excited.

  
Here's the graduate.
We're very proud of you, son.

  
A perfect report card, all B's.

  
Very proud.

  
Ma! I got a thing going here.

  
- You got lint on your fuzz.
- Ow! That's me!

  
- Wave to us! We'll be in row 118,000.
- Bye!

  
Barry, I told you,
stop flying in the house!

  
- Hey, Adam.
- Hey, Barry.

  
- Is that fuzz gel?
- A little. Special day, graduation.

  
Never thought I'd make it.

  
Three days grade school,
three days high school.

  
Those were awkward.

  
Three days college. I'm glad I took
a day and hitchhiked around the hive.

  
You did come back different.

  
- Hi, Barry.
- Artie, growing a mustache? Looks good.

  
- Hear about Frankie?
- Yeah.

  
- You going to the funeral?
- No, I'm not going.

  
Everybody knows,
sting someone, you die.

  
Don't waste it on a squirrel.
Such a hothead.

  
I guess he could have
just gotten out of the way.

  
I love this incorporating
an amusement park into our day.

  
That's why we don't need vacations.

  
Boy, quite a bit of pomp...
under the circumstances.

  
- Well, Adam, today we are men.
- We are!

  
- Bee-men.
- Amen!

  
Hallelujah!

  
Students, faculty, distinguished bees,

  
please welcome Dean Buzzwell.

  
Welcome, New Hive Oity
graduating class of...

  
...9:15.
*/