using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Foster
{
    public class UI_Controller : MonoBehaviour
    {
        public Scrollbar healthBar;
        public Scrollbar bossBar;

        void Update()
        {
            healthBar.size = (PlayerMovement.health / 100);
            bossBar.size = (EnemyController.health / 100);
        }
    }
}