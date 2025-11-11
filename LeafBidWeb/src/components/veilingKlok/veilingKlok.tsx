import s from "@/components/veilingKlok/veilingKlok.module.css";

type VeilingKlok = {
locatie: string;
bedrag: string;
tijd: string;
};

export default function VeilingKlok({
    locatie,
    bedrag,
    tijd,
                                    }: VeilingKlok) {
    return (
        <div className={`d-flex flex-column p-4 w-100 h-100 text-black{s.container}`}>
            <h2 className={`{s.locatie}`}>{locatie}</h2>
            <h1 className={`{s.bedrag}`}>{bedrag}</h1>
            <div className={`d-flex flex-column gap-2 {s.klokcontainer}`}>
                <div className={`w-100 h-100{s.balk}`}></div>
                <p className={`{s.tijd}`}>{tijd}</p>
            </div>
        </div>

    );

}