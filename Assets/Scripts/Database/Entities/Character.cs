using System;
using System.Collections;
using System.Linq;


class Character
{
    public string Name { get; set; }
    public int HP { get; set; }
    public int ATT { get; set; }
    public int DEF { get; set; }
    public int EVA { get; set; }
    public int CRIT { get; set; }
    public int MOV { get; set; }
    // public virtual House House { get; private set; }

    public Character() {
        this.loadCharacter();
    }

    private bool loadCharacter() {
        return true;
    }
}
