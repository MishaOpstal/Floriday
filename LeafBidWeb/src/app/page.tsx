import styles from './page.module.css';
import Header from "@/components/header/header";
import Knop from "@/components/knop/knop";
import DashboardPanel from "@/components/dashboardPanel/dashboardpanel";


export default function Home() {
  return (
      <>
          <Header></Header>
          <main className={styles.main}>

              <div className={styles.page}>
                  <h1 className={styles.huidigeVeilingen}>Huidige Veilingen</h1>
                  <div className={styles.panels}>
                      <DashboardPanel
                          loading={false}
                          title="Tulpenmix 'Lentezon'"
                          kloklocatie="Klok 1 - Hal A"
                          imageSrc="/images/PIPIPOTATO.png"
                          resterendeTijd="2 min 15 sec"
                          huidigePrijs="€ 12,30"
                          aankomendProductNaam="random product"
                          aankomendProductStartprijs="€ 213"
                      />

                      <DashboardPanel
                          loading={false}
                          title="Rozenpakket 'Romance'"
                          kloklocatie="Klok 2 - Hal B"
                          imageSrc="/images/PIPIPOTATO.png"
                          resterendeTijd="1 min 45 sec"
                          huidigePrijs="€ 18,90"
                          aankomendProductNaam="random product"
                          aankomendProductStartprijs="€ 1050"
                      />

                      <DashboardPanel
                          loading={false}
                          title="Zomerboeket 'Veldkracht'"
                          kloklocatie="Klok 3 - Hal C"
                          imageSrc="/images/PIPIPOTATO.png"
                          resterendeTijd="3 min 00 sec"
                          huidigePrijs="€ 15,75"
                          aankomendProductNaam="random product"
                          aankomendProductStartprijs="€ 100"
                      />

                      <DashboardPanel
                          loading={false}
                          title="Orchidee 'Wit Elegance'"
                          kloklocatie="Klok 4 - Hal D"
                          imageSrc="/images/PIPIPOTATO.png"
                          resterendeTijd="2 min 30 sec"
                          huidigePrijs="€ 22,40"
                          aankomendProductNaam="random product"
                          aankomendProductStartprijs="€ 200"
                      />

                      <DashboardPanel
                          loading={false}
                          title="Gerbera Regenboog"
                          kloklocatie="Klok 1 - Hal A"
                          imageSrc="/images/PIPIPOTATO.png"
                          resterendeTijd="1 min 20 sec"
                          huidigePrijs="€ 10,50"
                          aankomendProductNaam="random product"
                          aankomendProductStartprijs="€ 214"
                      />

                      <DashboardPanel
                          loading={true}
                          title="Lelies 'Zomergeur'"
                          kloklocatie="Klok 2 - Hal B"
                          imageSrc="/images/PIPIPOTATO.png"
                          resterendeTijd="2 min 05 sec"
                          huidigePrijs="€ 16,80"
                          aankomendProductNaam="random product"
                          aankomendProductStartprijs="€ 500"
                      />
                  </div>

              </div>

          </main>

      </>

    );
}
