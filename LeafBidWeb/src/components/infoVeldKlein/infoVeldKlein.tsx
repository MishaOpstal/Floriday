import 'bootstrap/dist/css/bootstrap.min.css';

type InfoFieldProps = {
    naam: string;
    prijs: string;
    plaatje: string;
};

export default function InfoVeld({ naam, prijs, plaatje }: InfoFieldProps) {
    const imageSrc = `/${plaatje}`;

    return (
        <div className="d-flex align-items-center gap-3  border  border-2 border-black rounded bg-white w-100">
            <img
                src={imageSrc}
                alt={naam}
                className="img-fluid rounded"
                style={{ width: '154px', height: '154px', objectFit: 'cover' }}
            />
            <div className="d-flex flex-column justify-content-center ps-3 gap-2">
                <h6 className="m-0" style={{ fontSize: '28.8px', fontWeight: 400 }}>{naam}</h6>
                <p className="text-muted m-0" style={{ fontSize: '20px', fontWeight: 400 }}>{prijs}</p>
            </div>
        </div>
    );
}
