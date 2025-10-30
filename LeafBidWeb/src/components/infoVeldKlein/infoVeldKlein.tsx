import s from './infoVeldKlein.module.css';

type InfoFieldProps = {
    naam: string;
    prijs: number;
    plaatje: string;
};

export default function InfoVeld({ naam, prijs, plaatje }: InfoFieldProps) {
    // Dynamic image path from public folder
    const imageSrc = `/${plaatje}`;

    return (
        <div className={s.wrapper}>
            <img className={s.plaatje} src={imageSrc} alt={naam} />
            <div className={s.tekstContainer}>
                <h6 className={s.naam}>{naam}</h6>
                <p className={s.prijs}>{prijs}</p>
            </div>
        </div>
    );
}