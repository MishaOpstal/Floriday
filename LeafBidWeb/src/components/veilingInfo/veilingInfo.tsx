import s from '.veilinginfo.module.css';

type BigFieldProps = {
    naam: string;
    prijs: number;
    plaatje: string;
    duur: string;
    info: string;
    oogst: string;
    leverancier: string;
    regio: string;
    aantal: number;

};

export default function BigInfoVeld({ naam, prijs, plaatje, duur, info, oogst, leverancier, regio, aantal}: BigFieldProps) {
    // Dynamic image path from public folder
    const imageSrc = `/${plaatje}`;

    return (
        <div className={s.wrapper}>
            <img className={s.plaatje} src={imageSrc} alt={naam} />
            <div className={s.tekstContainer}>
                <h2 className={s.naam}>{naam}</h2>
                <p className={s.prijs}>{prijs}</p>
                <p className={s.duur}>{duur}</p>
                <p className={s.info}>{info}</p>
                <p className={s.oogst}>{oogst}</p>
                <p className={s.leverancier}>{leverancier}</p>
                <p className={s.regio}>{regio}</p>
                <p className={s.aantal}>{aantal}</p>

            </div>
        </div>
    );
}