'use client';

import React from 'react';
import { Card, Row, Col, Image, Button } from 'react-bootstrap';
import Placeholder from "react-bootstrap/Placeholder";

type DashboardPanelProps = {
    title?: string;
    kloklocatie?: string;
    imageSrc?: string;
    resterendeTijd?: string;
    huidigePrijs?: string;
    aankomendProductNaam?: string;
    aankomendProductStartprijs?: string;
    loading?: boolean;
};

const DashboardPanel: React.FC<DashboardPanelProps> = ({title, kloklocatie, imageSrc, resterendeTijd, huidigePrijs, aankomendProductNaam, aankomendProductStartprijs, loading = false,}) => {
    return (
        <Card className="d-flex flex-row">
            <Card.Img
                className="w-25 rounded"
                variant="left"
                src={imageSrc || "/images/PIPIPOTATO.png"}
            />
            <Card.Body className="w-100">
                <div className="d-flex gap-3">
                    {/* First block */}
                    <div className="flex-fill w-75">
                        {loading ? (
                            <>
                                <Placeholder as={Card.Title} animation="glow">
                                    <Placeholder xs={6} />
                                </Placeholder>
                                <Placeholder as={Card.Text} animation="glow">
                                    <Placeholder xs={7} /> <Placeholder xs={4} /> <Placeholder xs={5} />
                                </Placeholder>
                            </>
                        ) : (
                            <>
                                <Card.Title>{kloklocatie}</Card.Title>
                                <Card.Text>
                                    <span>Huidig product: {title}</span><br />
                                    <span>Resterende tijd: {resterendeTijd}</span><br />
                                    <span>Huidige prijs: {huidigePrijs}</span>
                                </Card.Text>

                            </>
                        )}
                    </div>

                    {/* Second block */}
                    <div className="flex-fill w-25">
                        {loading ? (
                            <>
                                <Placeholder as={Card.Title} animation="glow">
                                    <Placeholder xs={5} />
                                </Placeholder>
                                <Placeholder as={Card.Text} animation="glow">
                                    <Placeholder xs={6} /> <Placeholder xs={3} />
                                </Placeholder>
                            </>
                        ) : (
                            <>
                                <Card.Title>Aankomende producten</Card.Title>
                                <Card.Text>
                                    <p className="mb-0">{aankomendProductNaam}</p>
                                    <p className="mb-0" style={{ fontSize: "0.7rem" }}>
                                        Startprijs: {aankomendProductStartprijs}
                                    </p>
                                </Card.Text>
                            </>
                        )}
                    </div>
                </div>
            </Card.Body>
        </Card>
    );
};


export default DashboardPanel;
