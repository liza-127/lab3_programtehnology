namespace Lab5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //прокси
            Console.WriteLine("покси");
            IPic p1 = new Picproxy("cat.jpg");
            IPic p2 = new Picproxy("dog.jpg");


            Console.WriteLine("oбъекты создал пока ничего не грузилось");
            p1.Show();
            p1.Show();

            p2.Show();

            //адаптер
            Console.WriteLine("адаптер");
            OldCharger oldch = new OldCharger();
            ICharge ch = new ChargerAdapter(oldch);
            ch.Charge();


            //мост
            Console.WriteLine("мост");
            IDevice sam = new Samsung();
            IDevice lg = new LG();

            Remote r1 = new SimpleRemote(sam);
            Remote r2 = new SmartRemote(lg);

            r1.Power();
            r1.Switch(4);


            r2.Power();
            r2.Switch(6);
        }
    }
}


//proxy
interface IPic
{

    void Show();
}

class Bigpic : IPic
{
    string name;


    public Bigpic(string n)
    {

        name = n;

        Console.WriteLine("загружаем картинку " + name + "...");
    }

    public void Show()
    {
        Console.WriteLine("показываем " + name);
    }
}



class Picproxy : IPic
{
    string name;
    Bigpic realpic;

    public Picproxy(string n)
    {
        name = n;
    }


    public void Show()
    {
        if (realpic == null)
        {
            realpic = new Bigpic(name);
        }


        realpic.Show();
    }
}




//adapter
interface ICharge
{
    void Charge();
}

class OldCharger
{
    public void OldCharge()
    {

        Console.WriteLine("заряжаем старым проводом ");
    }
}

class ChargerAdapter : ICharge
{
    OldCharger old;

    public ChargerAdapter(OldCharger o)
    {
        old = o;
    }

    public void Charge()
    {
        Console.WriteLine("ипользуем переходник ");
        old.OldCharge();
    }
}


//bridge
interface IDevice
{
    void On();
    void SetCh(int ch);
}

class Samsung : IDevice
{
    public void On()
    {


        Console.WriteLine("самсунг включился ");
    }
    public void SetCh(int ch) 
    { 
        Console.WriteLine("самсунг канал " + ch);
    }
}

class LG : IDevice
{
    public void On()
    {
        Console.WriteLine("LG включился"); 
    }
    public void SetCh(int ch) 
    {

        Console.WriteLine("LG канал " + ch);
    }
}

abstract class Remote
{
    protected IDevice dev;

    public Remote(IDevice d)
    {
        dev = d;
    }

    public abstract void Power();


    public abstract void Switch(int ch);
}


class SimpleRemote : Remote
{
    public SimpleRemote(IDevice d) : base(d)
    {

    }

    public override void Power()
    {
        Console.WriteLine("жму кнопку вкл на обычном пульте");
        dev.On();
    }

    public override void Switch(int ch)
    {
        Console.WriteLine("набираю канал" + ch);
        dev.SetCh(ch);
    }


}



class SmartRemote : Remote
{
    public SmartRemote(IDevice d) : base(d)
    {

    }

    public override void Power()
    {
        Console.WriteLine("говорю включись в умный пульт");
        dev.On();
    }


    public override void Switch(int ch)
    {
        Console.WriteLine("голосом ищу канал " + ch);
        dev.SetCh(ch);
    }


}
