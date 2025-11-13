import InfoVeld from "@/components/infoVeldKlein/infoVeldKlein";
import BigInfoVeld from "@/components/veilingInfo/veilingInfo";
import Header from "@/components/header/header";
import VeilingKlok from "@/components/veilingKlok/veilingKlok";
import s from "./page.module.css"
import {Product} from "@/types/Product";

// Dummy data
const fakeHarvestedAt = new Date();
fakeHarvestedAt.setDate(fakeHarvestedAt.getDate() - 14);

const products: Product[] = [
    {id: 1, name: "Rose", picture: "images/bloem.png", minPrice: 265, stock: 12},
    {id: 2, name: "Tulip", picture: "images/bloem.png", minPrice: 22.67, stock: 8},
    {id: 3, name: "Daisy", picture: "images/bloem.png", minPrice: -20, stock: 15},
    {
        id: 4,
        name: "Sunflower",
        picture: "images/bloem.png",
        minPrice: 22.6761,
        stock: 6,
        weight: 22,
        region: "Holland",
        maxPrice: 55,
        species: "Deadly LeaveWhisperer",
        stemLength: 5,
        harvestedAt: fakeHarvestedAt,
        description: "De aardappelplant vormt ondergronds eetbare knollen",
        potSize: 10,
        providerId: 1,
        auctionId: 1
    },
];


export default function Profile() {
    return (
        <> <Header returnOption={true}/>
            <main className={s.main}>
                <div className={s.links}>
                    {/*<VeilingKlok bedrag={"$5000"} locatie={"Zuid Holland"} tijd={"1:10"}/>*/}

                    <h3 className="fw-bold">Volgende Producten:</h3>
                    <div className={s.tekstblokken}>
                        <InfoVeld product={products[0]}/>
                        <InfoVeld product={products[1]}/>
                        <InfoVeld product={products[2]}/>
                    </div>
                </div>
                <div className={s.infoblok}>
                    <h3 className="fw-bold mb-2">Huidig Product:</h3>
                    <BigInfoVeld product={products[3]}/>
                </div>
            </main>
        </>
    );
}
