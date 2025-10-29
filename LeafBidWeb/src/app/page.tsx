import Header from "@/components/header/header";
import Knop from "@/components/knop/knop";
import DashboardPanel from "@/components/dashboardPanel/dashboardpanel";

export default function Home() {
  return (
      <>
          <Header/>

          <DashboardPanel
              title="Product 1"
              info="Dit is een korte beschrijving van product 1."
              imageSrc="/images/bloem.png"
          >
              <Knop label="Doe een bod" to="" />
          </DashboardPanel>
      </>

);
}
