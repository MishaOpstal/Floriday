import s from '@/components/infoVeldKlein/infoVeldKlein.module.css';

type InfoFieldProps = {
    naam: string;
    prijs: string;
    plaatje: string;
};

export default function InfoVeld({ naam, prijs, plaatje }: InfoFieldProps) {
    const imageSrc = `/${plaatje}`;

    return (
        <div className={`d-flex align-items-center gap-3 ${s.textContainer}`}>
            <img
                src={imageSrc}
                alt={naam}
                className={`img-fluid ${s.plaatje}`}
            />
            <div className="d-flex flex-column justify-content-center ps-3 gap-2">
                <h6 className="m-0" style={{ fontSize: '28.8px', fontWeight: 400 }}>{naam}</h6>
                <p className="text-muted m-0" style={{ fontSize: '20px', fontWeight: 400 }}>{prijs}</p>
            </div>
        </div>
    );
}
