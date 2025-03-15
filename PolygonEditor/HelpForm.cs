namespace PolygonEditor
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            helpTextBox.Text =
                """
                Instrukcje:
                • Aby utworzyć nowy punkt, kliknij w dowolne miejsce na płótnie. Jest to możliwe jedynie podczas rysowania, tzn. gdy na płótnie nie znajduje się zamknięty wielokąt.
                • Aby zakończyć rysowanie naciśnij na pierwszy punkt wielokąta.
                • Aby przesunąć wielokąt, naciśnij prawy przycisk myszy z dala od wierzchołków i krawędzi wielokąta oraz przesuń kursor.
                • Aby wyświetlić menu kontekstowe wierzchołka, naciśnij prawy przycisk myszy na wierzchołku.
                • Aby wyświetlić menu kontekstowe krawędzi, naciśnij prawy przycisk myszy na krawędzi.


                Opis zaimplementowanego algorytmu "relacji": 
                W przypadku przesuwania wierzchołka lub dodania ograniczenia sprawdzane są krawędzie w obie strony - najpierw w jedną, następnie w drugą stronę - aż do momentu, gdy nie jest potrzebne dalsze ich poprawianie. Jeśli zmiana nie jest możliwa, w przypadku przesuwania wierczhołka przesuwany jest cały wielokąt, a w przypadku dodawania ograncizenia wyświetlany jest komunikat.
                """;
        }
    }
}
